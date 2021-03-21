
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PDoctors
    {
        public class PGetDoctors
        {
        
            public short offset { get; set; }
        }
        public class PGetDoctorsDetails
        {
            public short offset { get; set; }
            public string specialty { get; set; }
            public string name { get; set; }
        }
    
        public class PGetDoctorsInfo
        {
            
            public string doccode { get; set; }
        }
    }
}
