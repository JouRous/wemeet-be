using System.IO;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using API.Configs;
using Microsoft.OpenApi.Models;
using Application.Services;
using Infrastructure;
using System.Reflection;
using Application.Utils;
using Application;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServices(_config);

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddInfrastructureServices(_config);
            services.AddApplicationServices();

            services.AddCors(options =>
                        {
                            options.AddPolicy("CorsPolicy", builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                        });
            services.AddSignalR();
            services.AddMvcCore()
                .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseCors("CorsPolicy");

            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();

            string avatarPath = Path.Combine(Directory.GetCurrentDirectory(), @"Uploads/Avatars");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Uploads/Files");

            if (!Directory.Exists(avatarPath))
            {
                Directory.CreateDirectory(avatarPath);
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(avatarPath),
                RequestPath = new PathString("/uploads/avatars")
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(filePath),
                RequestPath = new PathString("/uploads/files")
            });

            app.UseRouting();



            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationService>("/notification");
            });

        }
    }
}
