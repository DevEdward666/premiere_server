using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class mdlProc
    {
        public string proccode { get; set; }
        public string appointmentno { get; set; }
        public string procdesc { get; set; }
        public string dateencoded { get; set; }
        public string regprice { get; set; }
        public string resurl { get; set; }
        public string resnotes { get; set; }
        public string resfilename { get; set; }
        public Nullable<Decimal> availprice { get; set; }
        public string encodedby { get; set; }
        public int procsts { get; set; }

    }
}
