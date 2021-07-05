using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}