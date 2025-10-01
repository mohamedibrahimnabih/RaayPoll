namespace RaayPoll.API.Services.IServices
{
    public interface IAuthService
    {
        Task<AccessTokenResponse?> ValidateAndGenerateTokenAsync(string email, string password);
    }
}
