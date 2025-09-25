using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RaayPoll.API
{
    public static class AppConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Connection String
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity
            services.AddIdentityApiEndpoints<IdentityUser>()
             .AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>();

            // Business Services
            services.AddScoped<IPollService, PollService>();

            // FluentValidation
            //builder.Services.AddScoped<IValidator<PollRequest>, PollRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<PollRequestValidator>();

            // Mapster
            services.RegisterMapsterConfiguration();
            //TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
