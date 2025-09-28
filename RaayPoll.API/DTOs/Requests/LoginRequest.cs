using System.ComponentModel.DataAnnotations;

namespace RaayPoll.API.DTOs.Requests
{
    public class LoginRequest
    {
        [EmailAddress]
        public required string Email { get; set; } 
        public required string Password { get; set; } 
    }
}
