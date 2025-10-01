using Microsoft.EntityFrameworkCore;

namespace RaayPoll.API.Models
{
    public interface IAuditable<T>
    {
        public T AuditRecord { get; set; }
    }

    [Owned]
    public class AuditRecord
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public ApplicationUser CreatedBy { get; set; } = default!;

        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedById { get; set; }
        public ApplicationUser LastModifiedBy { get; set; } = default!;
    }
}
