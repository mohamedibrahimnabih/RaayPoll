
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaayPoll.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1NGk88rJYltERNfunPwwy0ULb8kKp8tJ"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7253",
                audience: "https://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), (long)(token.ValidTo - DateTime.UtcNow).TotalSeconds);
        }
    }
}
