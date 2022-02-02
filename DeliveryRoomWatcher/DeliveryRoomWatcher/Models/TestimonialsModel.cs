using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class TestimonialsModel
    {
        public class create_testimonials { 
            public string description { get; set; }
            public string author { get; set; }
        }
        public class update_testimonials
        {
            public short id { get; set; }
            public string description { get; set; }
            public string author { get; set; }
        }
    }
}
