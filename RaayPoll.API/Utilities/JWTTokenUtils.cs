using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaayPoll.API.Utilities
{
    public static class JWTTokenUtils
    {
        public static (string, long) GenerateJwtToken(string email, JWTOptions options)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(options.ExpiresInMinute),
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), (long)(token.ValidTo - DateTime.UtcNow).TotalSeconds);
        }
    }
}
