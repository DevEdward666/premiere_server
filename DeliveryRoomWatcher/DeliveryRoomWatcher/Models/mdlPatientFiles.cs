using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class mdlPatientFiles
    {
        public class insertfile
        {
            public string prem_id { get; set; }
            public string filename { get; set; }
            public string type { get; set; }
            public string deviceStored { get; set; }
        }
        public class getfile
        {
            public string prem_id { get; set; }
            public string type { get; set; }
            public string deviceStored { get; set; }
        }
    }
}
