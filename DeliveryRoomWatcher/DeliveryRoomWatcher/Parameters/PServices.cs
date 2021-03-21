using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PServices
    {
        public class PGetServices
        {
            [Required]
            [Display(Name = "Offset")]
            public short offset { get; set; }
        }
    }
}
