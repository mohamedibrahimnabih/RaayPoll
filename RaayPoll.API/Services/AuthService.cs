
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaayPoll.API.Services
{
    public class AuthService(SignInManager<ApplicationUser> signInManager, IOptions<JWTOptions> options, ITokenService tokenService, UserManager<ApplicationUser> userManager) : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JWTOptions _options = options.Value;
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private readonly int _refreshTokenExpiryTime = 7;

        public async Task<bool> RevokeTokenAsync(string accessToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            if (principal is null)
                return false;

            var username = principal.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

            //if (username is null)
            //    return null;

            var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);

            if (user is null)
                return false;

            var storedToken = user.RefreshTokens.SingleOrDefault(e => e.RefreshTokenKey == refreshToken);

            if (storedToken is null || storedToken.RefreshTokenKey != refreshToken || storedToken.RefreshTokenExpiryTime <= DateTime.Now)
                return false;

            //storedToken.RefreshTokenKey = null;
            storedToken.RefreshTokenExpiryTime = DateTime.MinValue;
            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<AccessTokenResponse?> ValidateAndGenerateTokenAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);

            if(!result.Succeeded)
                return null;

            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
                return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(new()
            {
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryTime),
                RefreshTokenKey = refreshToken,
            });

            await _userManager.UpdateAsync(user);

            return new() { AccessToken = accessToken, ExpiresInMinute = _options.ExpiresInMinute, RefreshToken = refreshToken };
        }

        public async Task<AccessTokenResponse?> ValidateTokenAndGenerateNewAsync(string accessToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            if(principal is null)
                return null;

            var username = principal.Claims.FirstOrDefault(e=>e.Type == ClaimTypes.NameIdentifier)?.Value;

            //if (username is null)
            //    return null;

            var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);

            if (user is null)
                return null;

            var storedToken = user.RefreshTokens.SingleOrDefault(e => e.RefreshTokenKey == refreshToken);

            if (storedToken is null || storedToken.RefreshTokenKey != refreshToken || storedToken.RefreshTokenExpiryTime <= DateTime.Now)
                return null;

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Remove(storedToken);

            user.RefreshTokens.Add(new()
            {
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryTime),
                RefreshTokenKey = newRefreshToken,
            });
            await _userManager.UpdateAsync(user);

            return new() { AccessToken = newAccessToken, ExpiresInMinute = _options.ExpiresInMinute, RefreshToken = newRefreshToken };
        }
    }
}
