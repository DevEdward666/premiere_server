using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class ServicesModel
    {
        public class Create_Services
        {
            public string services_id { get; set; }
            public string hosp_serv_name { get; set; }
            public string hosp_serv_description { get; set; }
            public IFormFile attach_files { set; get; }
        }
        public class Create_Services_desc
        {
            public string services_id { get; set; }
            public string serv_desc_id { get; set; }
            public string title { get; set; }
            public string desc { get; set; }
            public IFormFile attach_files { set; get; }
        }
        public class Create_Services_info
        {
            public string services_id { get; set; }
            public string img { get; set; }
            public string subject { get; set; }
            public string description { get; set; }
            public string published { get; set; }
            public IFormFile attach_files { set; get; }
        }
    }
}
