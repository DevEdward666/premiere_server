using CorePush.Apple;
using CorePush.Google;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hubs;
using DeliveryRoomWatcher.Models.FCM;
using DeliveryRoomWatcher.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
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
            DefaultConfig.app_name = Configuration["DEFAULTS:app_name"];

            DefaultConfig.paymongo_secret_key = Configuration["PAY:paymongo_secret_key"];
            DefaultConfig.paymongo_public_key = Configuration["PAY:paymongo_public_key"];
            DefaultConfig.paymongo_payment_url = Configuration["PAY:paymongo_payment_url"];
            DefaultConfig.paymongo_source_url = Configuration["PAY:paymongo_source_url"];
            DefaultConfig.paymongo_pay_intent_url = Configuration["PAY:paymongo_pay_intent_url"];

            DefaultConfig.passbase_public_key = Configuration["PASSBASE:passbase_public_key"];
            DefaultConfig.passbase_secret_key = Configuration["PASSBASE:passbase_secret_key"];
            DefaultConfig.passbase_verification_url = Configuration["PASSBASE:passbase_verification_url"];

            DefaultConfig._providerEmailAddress = Configuration["EMAIL:_providerEmailAddress"];
            DefaultConfig._providerEmailPass = Configuration["EMAIL:_providerEmailPass"];
            DefaultConfig._clientBaseUrl = Configuration["EMAIL:_clientBaseUrl"];

            DefaultConfig.ServerKey = Configuration["FcmNotificationSetting:ServerKey"];
            DefaultConfig.SenderId = Configuration["FcmNotificationSetting:SenderId"];
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
            services.InstallServicesInAssembly(Configuration);
            services.AddSignalR();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();
            var appSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors(builder => builder
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader());

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
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/Services")),
                RequestPath = "/Resources/Services"

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/Testimonials")),
                RequestPath = "/Resources/Testimonials"

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/AppIcons")),
                RequestPath = "/Resources/AppIcons"

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/Announcements")),
                RequestPath = "/Resources/Announcements"

            });      
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/UserImages")),
                RequestPath = "/Resources/UserImages"

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/Meetings")),
                RequestPath = "/Resources/Meetings"

            });
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
