using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class mdlNotifications
    {
        public string name { get; set; }
        public short offset { get; set; }
        public class createnotifications
        {
            public string created_by{ get; set; }
            public string audience{ get; set; }
            public string priority{ get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }
        public class searchNotif
        {
            public string priority { get; set; }
            public string title { get; set; }
            public int offset { get; set; }
        }
    }
}
