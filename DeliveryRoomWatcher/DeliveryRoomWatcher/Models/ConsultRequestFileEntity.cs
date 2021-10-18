using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class ConsultRequestFileEntity
    {
        public int cr_file_pk { get; set; }
        public string consult_req_pk { get; set; }
        public string file_dest { get; set; }
        public string file_name { get; set; }
        public DateTime? encoded_at { get; set; }
    }
}
