using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PNews
    {
        public class PGetNews
        {
            public short offset { get; set; }
            public short month { get; set; }
            public short year { get; set; }
        }
        public class PGetNewsInfo
        {
            public string id { get; set; }
        }
        public class PGetNewsWeek
        {
            public short offset { get; set; }
        }
        public class PGetNewsToday
        {
            public short offset { get; set; }
        }

    }
}
