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
using API.Services;
using API.Configs;
using Microsoft.OpenApi.Models;

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
            services.AddAppServices(_config);
            services.AddIdentityServices(_config);
            services.AddControllers();
            services.AddCors(options =>
                        {
                            options.AddPolicy("CorsPolicy", builder => builder
                        .WithOrigins(_config["FE_URL"])
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

            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Uploads/Avatars")
              ),
                RequestPath = new PathString("/uploads/avatar")
            });

            app.UseRouting();

            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseCors("CorsPolicy");

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
