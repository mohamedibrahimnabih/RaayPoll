
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaayPoll.API.Services
{
    public class AuthService(SignInManager<ApplicationUser> signInManager, IOptions<JWTOptions> options) : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JWTOptions _options = options.Value;

        public async Task<AccessTokenResponse?> ValidateAndGenerateToken(string email, string password, CancellationToken cancellationToken = default)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);

            if(!result.Succeeded)
                return null;

            var (token, expireIn) = GenerateJwtToken(email);

            return new() { AccessToken = token, ExpiresIn = expireIn };
        }

        private (string, long) GenerateJwtToken(string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_options.ExpiresInMinute),
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), (long)(token.ValidTo - DateTime.UtcNow).TotalSeconds);
        }
    }
}
