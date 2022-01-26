using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Employee
{
    public class AnnouncementModel
    {
        public class Create_Announcements
        {
            public string announcement_id { get; set; }
            public string img { get; set; }
            public string subject { get; set; }
            public string description { get; set; }
            public string published { get; set; }
            public List<IFormFile> attach_files { set; get; }
        }
        public class Get_Announcement_images
        {
            public string announcement_id { get; set; }
           
        }     
        public class filter_announcement
        {
            public string published { get; set; }
            public string month { get; set; }
            public string year { get; set; }
           
        }
        public class Unpublished_Announcement
        {
            public string announcement_id { get; set; }

        }
        public class update_Announcement
        {
            public string announcement_id { get; set; }
            public string subject { get; set; }
            public string description { get; set; }
            public string published { get; set; }

        }
    }
}
