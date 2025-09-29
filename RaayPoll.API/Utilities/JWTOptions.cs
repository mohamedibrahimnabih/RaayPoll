using System.ComponentModel.DataAnnotations;

namespace RaayPoll.API.Utilities
{
    public class JWTOptions
    {
        public const string JWT = "JWT";

        public string Key { get; set; } = String.Empty;
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        [Range(1, int.MaxValue)]
        public int ExpiresInMinute { get; set; }
    }
}
