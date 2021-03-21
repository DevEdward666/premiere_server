using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PAuthUser
    {

        [Required]
        [StringLength(20)]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Password")]
        public string password { get; set; }
    }
}
