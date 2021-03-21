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
        [Required]
        [StringLength(150)]
        [Display(Name = "URLDOCS")]
        public string url_docs { get; set; }
        [Required]
        [StringLength(150)]
        [Display(Name = "URL")]
        public string url { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Firstname")]
        public string firstname { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Middlename")]
        public string middlename { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Lastname")]
        public string lastname { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Birthdate")]
        public string birthdate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string username { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Required]
        [StringLength(13)]
        [Display(Name = "Mobileno")]
        public string mobileno { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Regioncode")]
        public string region_code { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Citycode")]
        public string city_code { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Provincecode")]
        public string province_code { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Barangaycode")]
        public string barangay_code { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Nationalitycode")]
        public string nationality_code { get; set; }
        [Required]
        [StringLength(150)]
        [Display(Name = "Fulladdress")]
        public string fulladdress { get; set; }
        [Required]
        [StringLength(6)]
        [Display(Name = "Zipcode")]
        public string zipcode { get; set; }


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
