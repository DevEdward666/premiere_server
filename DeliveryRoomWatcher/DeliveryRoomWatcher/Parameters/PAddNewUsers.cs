using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PAddNewUsers
    {
        public string url_docs { get; set; }
        public string url { get; set; }
        public string firstname { get; set; }

        public string middlename { get; set; }

        public string lastname { get; set; }

        public string gender { get; set; }

        public string birthdate { get; set; }

        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string pin { get; set; }
        public string mobileno { get; set; }
        public string region_code { get; set; }
        public string city_code { get; set; }
        public string province_code { get; set; }
 
        public string barangay_code { get; set; }
    
        public string nationality_code { get; set; }
      
        public string fulladdress { get; set; }
       
        public string zipcode { get; set; }


    }
    public class mdlLocked
    {
        
        public string username { get; set; }
        public string islocked { get; set; }
    }
    public class PGetUsername
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string username { get; set; }
    }
    public class PAddNewOTP
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "username")]
        public string username { get; set; }
    }
    public class PSetProfile
    {
        [Required(ErrorMessage = "Please choose profile image")]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
    }

}
