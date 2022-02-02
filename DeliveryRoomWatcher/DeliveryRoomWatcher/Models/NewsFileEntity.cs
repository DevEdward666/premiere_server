using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class NewsFileEntity
    {
        public string news_id { get; set; }
        public string news_image { get; set; }
        public string news_image_name { get; set; }
        public DateTime? dateEncoded { get; set; }
    }
}
