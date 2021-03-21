using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PDefaults
    {
    
        [StringLength(50)]
        [Display(Name = "Regioncode")]
        public string region_code { get; set; }
  
        [StringLength(50)]
        [Display(Name = "Citycode")]
        public string city_code { get; set; }

        [StringLength(50)]
        [Display(Name = "Provincecode")]
        public string province_code { get; set; }
      
        [StringLength(50)]
        [Display(Name = "Barangaycode")]
        public string barangay_code { get; set; }
 
        [StringLength(50)]
        [Display(Name = "Nationalitycode")]
        public string nationality_code { get; set; }
    }
}
