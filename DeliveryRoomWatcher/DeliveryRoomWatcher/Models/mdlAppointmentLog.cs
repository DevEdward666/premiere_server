using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class mdlAppointmentLog
    {
        public int logno { get; set; }
        public string appointmentno { get; set; }
        public string logevent { get; set; }
        public int stsid { get; set; }
        public Nullable<DateTime> encodedat { get; set; }
        public string encodedby { get; set; }
    }
}
