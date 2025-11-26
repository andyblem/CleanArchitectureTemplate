using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Settings;
using CleanArchitecture.Infrastructure.Shared.DTOs;
using CleanArchitecture.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<FileSettingsDTO>(config.GetSection("FileSettings"));
            services.Configure<MailSettings>(config.GetSection("MailSettings"));

            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IFileService, FileService>();
        }
    }
}
