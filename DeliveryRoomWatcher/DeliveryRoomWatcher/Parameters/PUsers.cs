using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PUsers
    {
        public class PGetUsers
        {
            [Required]
            [Display(Name = "Offset")]
            public short offset { get; set; }
        }
        public class PGetUsersInfo
        {
            [Required]
            [StringLength(30)]
            [Display(Name = "Premid")]
            public string premid { get; set; }
        }
        public class PGetSingleMedicalRecords
        {
            
            public string hospitalno { get; set; }
        }
        public class PUpdateUserInfo
        {
            [Required]
            [StringLength(30)]
            [Display(Name = "Premid")]
            public string premid { get; set; }
        }

        public class PUpdateLinkMedicalRecords
        {
          
            public string premid { get; set; }
            public string patno { get; set; }
            public string status { get; set; }
        }
        public class PAddDiagnosticAppointment
        {
            public string premid { get; set; }
            public string reason { get; set; }

            public List<PAddDiagnosticAppointmentProcedure> listofprocedures { get; set; }

        }
        public class PAddDiagnosticAppointmentOthers
        {
            public string premid { get; set; }
            public string prefix { get; set; }
            public string firstname { get; set; }
            public string middlename { get; set; }
            public string lastname { get; set; }
            public string suffix { get; set; }
            public string gender { get; set; }
            public string civil_status { get; set; }
            public string nationality_code { get; set; }
            public string religion_code { get; set; }
            public string birthdate { get; set; }
            public string email { get; set; }

            public string mobile { get; set; }
            public string fulladdress { get; set; }
            public string fulladdress2 { get; set; }
            public string barangay { get; set; }

            public string province_code { get; set; }
            public string city_code { get; set; }
            public string region_code { get; set; }

            public string zipcode { get; set; }
            public string reason { get; set; }
            public List<PAddDiagnosticAppointmentProcedure> listofprocedures { get; set; }

        }
        public class PAddDiagnosticAppointmentProcedure
        {
            public string premid { get; set; }
            public string reason { get; set; }
            public string proccode { get; set; }
            public string proccost { get; set; }
        } 
 
        public class PGetUsersImage
        {
            [Required]
            [StringLength(255)]
            [Display(Name = "Username")]
            public string username { get; set; }
        }
    }
}
