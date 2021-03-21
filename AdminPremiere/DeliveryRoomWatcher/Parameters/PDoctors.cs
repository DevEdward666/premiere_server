
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PDoctors
    {
        public class PGetDoctors
        {
            [Required]
            [Display(Name = "Offset")]
            public short offset { get; set; }
        }
        public class PGetDoctorsDetails
        {
            [Required]
            [Display(Name = "Offset")]
            public short offset { get; set; }
            [Required]
            [StringLength(20)]
            [Display(Name = "Specialty")]
            public string specialty { get; set; }
        }
    
        public class PGetDoctorsInfo
        {
            [Required]
            [StringLength(20)]
            [Display(Name = "Doccode")]
            public string doccode { get; set; }
        }
    }
}
