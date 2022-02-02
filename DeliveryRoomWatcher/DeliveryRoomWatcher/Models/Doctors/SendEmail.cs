using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Doctors
{
    public class SendEmail
    {
        public string Subject { get; set; }
        public string RecieversName { get; set; }
        public string To { get; set; }
        public string From{ get; set; }
        public string Body { get; set; }

        public List<IFormFile> attachments { set; get; }
    }
}
