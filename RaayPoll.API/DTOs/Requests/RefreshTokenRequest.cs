using System.ComponentModel.DataAnnotations;

namespace RaayPoll.API.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
