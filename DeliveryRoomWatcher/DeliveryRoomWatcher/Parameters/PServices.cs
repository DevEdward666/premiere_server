using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PServices
    {
        public class PGetServices
        {
            public short offset { get; set; }
        }
        public class GetServiceID
        {
            public string service_id { get; set; }
        }
        public class GetServiceDescID
        {
            public string service_desc_id { get; set; }
        }
    }
}
