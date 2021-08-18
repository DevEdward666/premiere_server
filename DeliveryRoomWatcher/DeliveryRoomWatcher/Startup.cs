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
            DefaultConfig.ftp_ip = Configuration["FTP:ip"];
            DefaultConfig.ftp_port = Configuration["FTP:port"];
            DefaultConfig.ftp_user = Configuration["FTP:user"];
            DefaultConfig.ftp_pass = Configuration["FTP:pass"];
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                                  });
            });
            //services.AddCors();
            services.InstallServicesInAssembly(Configuration);
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseCors(builder => builder.WithOrigins("http://localhost:5020")
              .SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              );


            //var swaggerConfig = new SwaggerConfig();
            //Configuration.GetSection(nameof(SwaggerConfig)).Bind(swaggerConfig);
            //app.UseSwagger(option =>
            //{
            //    option.RouteTemplate = swaggerConfig.JsonRoute;
            //});

            //app.UseSwaggerUI(option =>
            //{
            //    option.SwaggerEndpoint(swaggerConfig.UIEndpoint, swaggerConfig.Description);
            //});
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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("api/message/message");
                endpoints.MapHub<NotifyHub>("api/notif/notify");

            });

        }


    }
}
