using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PDIagnostics
    {
        public string premid { get; set; }
        public short offset { get; set; }
        public class SearchProcedure {
            public string procedure { get; set; }
        }
    }
}
