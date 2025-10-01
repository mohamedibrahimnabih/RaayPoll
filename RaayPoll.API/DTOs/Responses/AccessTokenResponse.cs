namespace RaayPoll.API.DTOs.Responses
{
    public class AccessTokenResponse
    {
        public required string AccessToken { get; init; }
        public required int ExpiresInMinute { get; init; }
        public required string RefreshToken { get; init; }
    }
}
