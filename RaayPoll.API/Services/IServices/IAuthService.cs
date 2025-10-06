using FluentResults;
using System.Security.Claims;

namespace RaayPoll.API.Services.IServices
{
    public interface IAuthService
    {
        Task<Result<AccessTokenResponse>> ValidateAndGenerateTokenAsync(string email, string password);
        Task<Result<AccessTokenResponse>> ValidateTokenAndGenerateNewAsync(string accessToken, string refreshToken);
        Task<Result> RevokeTokenAsync(string accessToken, string refreshToken);
    }
}
