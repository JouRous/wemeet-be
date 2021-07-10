using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                var connStr = config.GetConnectionString("Default");

                options.UseNpgsql(connStr);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IMeetingRepo, MeetingRepository>();
            services.AddScoped<INotificationRepo, NotificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IFileRepository, FileRepository>();

            return services;
        }
    }
}