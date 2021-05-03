using System;
using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
  public static class AppServiceExtensions
  {
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();

      services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

      services.AddDbContext<AppDbContext>(options =>
      {
        var env = Environment.GetEnvironmentVariable("BUILD_ENV");
        var connStr = config.GetConnectionString("DefaultConnection");
        if (env != null)
        {
          connStr = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        }
        options.UseNpgsql(connStr);
      });

      return services;
    }
  }
}