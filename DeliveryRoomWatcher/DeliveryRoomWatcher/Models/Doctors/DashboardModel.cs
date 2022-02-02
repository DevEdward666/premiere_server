using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Doctors
{
    public class DashboardModel
    {
        public class NewPatients
        {
            public short idno { get; set; }
        }
        public class ActivePatients
        {
            public short idno { get; set; }
        }     
        public class patient_info
        {
            public string patno { get; set; }
        }         
        public class patient_hospitalno
        {
            public string hospitalno { get; set; }
        }   
        public class filter_patients
        {
            public string search { get; set; }
            public short idno { get; set; }
        }
        public class ListOfPatients
        {
            public short idno { get; set; }
        }
    }
}
