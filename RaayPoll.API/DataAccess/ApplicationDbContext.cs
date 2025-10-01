using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RaayPoll.API.DataAccess
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Poll> Polls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PollEntityTypeConfiguration).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();

            var now = DateTime.UtcNow;
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added && entry.Entity is IAuditable<AuditRecord> added)
                {
                    added.AuditRecord.CreatedAt = now;
                    added.AuditRecord.CreatedById = userId;

                }
                else if (entry.State == EntityState.Modified && entry.Entity is IAuditable<AuditRecord> modified)
                {
                    modified.AuditRecord.LastModifiedAt = now;
                    modified.AuditRecord.LastModifiedById = userId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
