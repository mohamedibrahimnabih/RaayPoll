using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaayPoll.API.Models
{
    [Owned, Table("RefreshTokens")]
    public class RefreshToken
    {
        public string? RefreshTokenKey { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
