using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class ImageToFileModel
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
