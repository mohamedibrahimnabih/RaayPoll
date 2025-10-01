
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

        public async Task<AccessTokenResponse?> ValidateAndGenerateTokenAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);

            if(!result.Succeeded)
                return null;

            var (token, expireIn) = JWTTokenUtils.GenerateJwtToken(email, _options);

            return new() { AccessToken = token, ExpiresIn = expireIn };
        }
    }
}
