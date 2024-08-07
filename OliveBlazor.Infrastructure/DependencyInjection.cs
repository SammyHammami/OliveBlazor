using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OliveBlazor.Core.Application.Services.Email;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Infrastructure.Indentity;
using OliveBlazor.Infrastructure.Services;

namespace OliveBlazor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddMemoryCache();

            services.AddScoped<IEmailService>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var userManager = sp.GetRequiredService<UserManager<UserIdentity>>();

                return new Smtp2GoEmailService(
                    configuration["EmailSettings:SmtpServer"],
                    Convert.ToInt32(configuration["EmailSettings:SmtpPort"]),
                    configuration["EmailSettings:UserName"],
                    configuration["EmailSettings:Password"],
                    configuration,
                    userManager
                );
            });

            return services;
        }
    }
}