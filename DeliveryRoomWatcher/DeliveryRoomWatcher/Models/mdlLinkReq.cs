using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class mdlLinkReq
    {
        public class request { 
        public string patno { get; set; }
            public string prem_id { get; set; }
            public string status { get; set; }
        }
    }
}
