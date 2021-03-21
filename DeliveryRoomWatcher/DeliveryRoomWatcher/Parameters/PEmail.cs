using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PEmail
    {
        [StringLength(50)]
        [Display(Name = "FromName")]
        public string fromname { get; set; }


        [StringLength(50)]
        [Display(Name = "ToName")]
        public string toname { get; set; }
        [StringLength(50)]
        [Display(Name = "From")]
        public string from { get; set; }


        [StringLength(50)]
        [Display(Name = "To")]
        public string to { get; set; }
    }
}
