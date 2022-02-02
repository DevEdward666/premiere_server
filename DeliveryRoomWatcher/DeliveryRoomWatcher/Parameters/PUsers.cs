using DeliveryRoomWatcher.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public class UpdateUserInfo
        {
            public string passbase_id { get; set; }
            public string passbase_status { get; set; }
            public string docs { get; set; }
            public string img { get; set; }
            public string username { get; set; }
            public string active { get; set; }
            public string civil_status { get; set; }
            public string region_code { get; set; }
            public string religion_code { get; set; }
            public string city_code { get; set; }
            public string province_code { get; set; }

            public string barangay_code { get; set; }

            public string nationality_code { get; set; }

            public string fulladdress { get; set; }

            public string zipcode { get; set; }

            public IFormFile profilefile { get; set; }
            public IFormFile Docsfile { get; set; }
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
            public string req_total_cost { get; set; }
            public DiagnosticRequestFileEntity req_file { get; set; }
            public StatusMasterEntity status { get; set; }
            public List<IFormFile> attach_req_files { set; get; }
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
            public string civil_status_key { get; set; }
            public string civil_status_desc { get; set; }
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

            public string psgc_address { get; set; }
            public string req_total { get; set; }

            public string zipcode { get; set; }
            public string reason { get; set; }
            public DiagnosticRequestFileEntity req_file { get; set; }
            public StatusMasterEntity status { get; set; }
            public List<IFormFile> attach_req_files { set; get; }
            public List<PAddDiagnosticAppointmentProcedure> listofprocedures { get; set; }

        }
        public class PAddDiagnosticAppointmentProcedure
        {

            public string proccode { get; set; }
            public string procdesc { get; set; }
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
