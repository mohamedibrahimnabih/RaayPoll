using System.Security.Claims;

namespace RaayPoll.API.Services.IServices
{
    public interface IAuthService
    {
        Task<AccessTokenResponse?> ValidateAndGenerateTokenAsync(string email, string password);
        Task<AccessTokenResponse?> ValidateTokenAndGenerateNewAsync(string accessToken, string refreshToken);
        Task<bool> RevokeTokenAsync(string accessToken, string refreshToken);
    }
}
