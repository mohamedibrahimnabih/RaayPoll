using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

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
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Authentication
            var JWTOptionsConfig = configuration.GetSection(JWTOptions.JWT).Get<JWTOptions>();

            //services.Configure<JWTOptions>(configuration.GetSection(JWTOptions.JWT));
            services.AddOptions<JWTOptions>()
                .Bind(configuration.GetSection(JWTOptions.JWT))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JWTOptionsConfig?.Issuer,
                        ValidAudience = JWTOptionsConfig?.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptionsConfig?.Key!))
                    };
                });

            // Business Services
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IAuthService, AuthService>();

            // FluentValidation
            //builder.Services.AddScoped<IValidator<PollRequest>, PollRequestValidator>();
            //services.AddValidatorsFromAssemblyContaining<PollRequestValidator>();

            // Mapster
            services.RegisterMapsterConfiguration();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
