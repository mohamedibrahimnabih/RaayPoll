using System.ComponentModel.DataAnnotations.Schema;

namespace RaayPoll.API.Models
{
    public class Poll : IAuditable<AuditRecord>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public bool IsPublished { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public AuditRecord AuditRecord { get; set; } = new AuditRecord();
    }
}
