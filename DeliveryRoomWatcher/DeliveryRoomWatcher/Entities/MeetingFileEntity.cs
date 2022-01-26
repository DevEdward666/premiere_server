using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Entities
{
    public class MeetingFileEntity
    {
        public string meeting_id { get; set; }
        public string file_dest { get; set; }
        public string file_name { get; set; }
        public DateTime? createdDate { get; set; }
    }
}
