using CleanArchitecture.Admin.Infrastructure.Persistance.Seeders;
using CleanArchitecture.Infrastructure.Persistence.Seeders;
using CleanArchitecture.Infrastructure.Shared.Identity.Managers;
using CleanArchitecture.Infrastructure.Shared.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistance.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceScope scope, IConfiguration configuration)
        {
            //Resolve ASP .NET Core Identity with DI help
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeeder>>();
            var userManager = scope.ServiceProvider.GetRequiredService<CustomUserManager<CustomIdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Audit.Core.Configuration.AuditDisabled = true;
            await RolesSeeder.SeedAsync(roleManager);
            await UsersSeeder.SeedAsync(userManager, roleManager, configuration, logger);
            Audit.Core.Configuration.AuditDisabled = false;
        }
    }
}
