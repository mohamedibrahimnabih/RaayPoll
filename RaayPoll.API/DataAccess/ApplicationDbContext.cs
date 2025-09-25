using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RaayPoll.API.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Poll> Polls { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PollEntityTypeConfiguration).Assembly);
        }
    }
}
