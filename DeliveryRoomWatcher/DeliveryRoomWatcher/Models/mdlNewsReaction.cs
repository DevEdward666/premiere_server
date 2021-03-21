using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class mdlNewsReaction
    {
        public class addReaction { 
        public string news_id { get; set; }
        public string reaction { get; set; }
        public string reactedby { get; set; }
        }
        public class getallReaction
        {
            public string news_id { get; set; }
        }
    }
}
