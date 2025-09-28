namespace RaayPoll.API.DTOs.Responses
{
    public class AccessTokenResponse
    {
        public required string AccessToken { get; init; }
        public required long ExpiresIn { get; init; }
    }
}
