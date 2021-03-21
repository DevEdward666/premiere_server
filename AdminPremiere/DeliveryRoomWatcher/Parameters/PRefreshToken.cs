using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PRefreshToken
    {
        [Required]
        [Display(Name = "Refresh Token")]
        public string RefreshToken { get; set; }
    }
}
