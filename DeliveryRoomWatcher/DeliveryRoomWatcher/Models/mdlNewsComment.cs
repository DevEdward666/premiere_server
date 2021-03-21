using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class mdlNewsComment
    {

        public class addcomment { 
        public string news_id { get; set; }
        public string comment { get; set; }
        public string commentedby { get; set; }
        }
        public class getallcomment
        {
            public string news_id { get; set; }
        }
    }
}
