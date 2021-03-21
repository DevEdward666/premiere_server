using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Common
{
    public class ResponseModel
    {
        [Required]
        public bool success { get; set; } = false;
        public object data { get; set; }
        public string message { get; set; }
        public List<ErrorModel> errors { get; set; }
        public string file { get; set; }
    }

    public class ErrorModel
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
