namespace RaayPoll.API.Services.IServices
{
    public interface IAuthService
    {
        Task<AccessTokenResponse?> ValidateAndGenerateToken(string email, string password, CancellationToken cancellationToken = default);
    }
}
