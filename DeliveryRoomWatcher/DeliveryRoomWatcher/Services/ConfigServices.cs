using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DeliveryRoomWatcher.Services
{
        public class ConfigServices : IServices
        {
            public void InstallServices(IServiceCollection services, IConfiguration configuration)
            {
                services.AddControllers();
                services.AddSignalR();


            }
        }
    }

