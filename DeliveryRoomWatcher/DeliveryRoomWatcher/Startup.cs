using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hubs;
using DeliveryRoomWatcher.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DeliveryRoomWatcher
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            DatabaseConfig.conStr = configuration.GetConnectionString("MysqlConnection");
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("http://localhost:4000").AllowAnyMethod().AllowAnyHeader();
            //                      });
            //});
            services.AddCors();
            services.InstallServicesInAssembly(Configuration);
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder => builder
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .SetIsOriginAllowed((host) => true)
                  .AllowCredentials()
              );


            var swaggerConfig = new SwaggerConfig();
            Configuration.GetSection(nameof(SwaggerConfig)).Bind(swaggerConfig);
            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerConfig.JsonRoute;
            });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerConfig.UIEndpoint, swaggerConfig.Description);
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/Images")),
                RequestPath = "/Resources/Images"

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/News")),
                RequestPath = "/Resources/News"

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/Events")),
                RequestPath = "/Resources/Events"

            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/message");
                endpoints.MapHub<NotifyHub>("/notify");
                endpoints.MapControllers();
            });

        }


    }
}
