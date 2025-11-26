using Audit.Core;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Infrastructure.Shared.Identity.Managers;
using CleanArchitecture.Infrastructure.Shared.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // register to DI
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();


            // get use in memory database configuration
            bool useInMemoryDatabase = Convert.ToBoolean(configuration.GetSection("UseInMemoryDatabase").Value);
            var authenticatedUserService = services.BuildServiceProvider().GetService<IAuthenticatedUserService>();

            // set db context
            if (useInMemoryDatabase == true)
            {
                // use in memory database
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));

            }
            else
            {
                // set connection string string
                // and server version
                var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                var serverVersion = ServerVersion.AutoDetect(connectionString);

                // get environment
                string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                bool isInDevMode = env == "Development";

                // set db context depending on mode
                if (isInDevMode == true)
                {
                    // add db context
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(
                            connectionString,
                            serverVersion,
                           b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                        // The following three options help with debugging, but should
                        // be changed or removed for production.
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors(),
                        ServiceLifetime.Scoped);
                }
                else
                {
                    // add db context
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(
                            connectionString,
                            serverVersion,
                           b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                           ServiceLifetime.Scoped);
                }

            }

            // use default identity provider
            services.AddIdentity<CustomIdentityUser, IdentityRole>(options =>
            {
                //password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;

                //lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<CustomUserManager<CustomIdentityUser>>()
            .AddDefaultTokenProviders();

            // Enable auditing
            Configuration.Setup()
                .UseMySql(config => config
                .ConnectionString(configuration.GetConnectionString("DefaultConnection"))
                .TableName("GeneralAudits")
                .IdColumnName("EntityId")
                .JsonColumnName("Data")
                .CustomColumn("User", ev => authenticatedUserService?.UserId ?? "System")
                .CustomColumn("InsertedDate", ev => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
                .CustomColumn("LastUpdatedDate", ev => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}
