using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.User
{
    public class DoctorModel
    {
        public string prem_id { get; set; }
        public string doccode { get; set; }
        public string lastname { get; set; }

        public string firstname { get; set; }
        public string middlename { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public class GetDoctorIdno
    {
        public string idno { get; set; }
    }
}
