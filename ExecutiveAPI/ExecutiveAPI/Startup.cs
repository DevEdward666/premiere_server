using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ExecutiveAPI.Config;
using ExecutiveAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExecutiveAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            DatabaseConfig.conStr = configuration.GetConnectionString("MysqlConnection");
            Configuration = configuration;
            DefaultConfig._providerEmailAddress = Configuration["EMAIL:_providerEmailAddress"];
            DefaultConfig._providerEmailPass = Configuration["EMAIL:_providerEmailPass"];

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
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors(builder => builder
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader());
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
