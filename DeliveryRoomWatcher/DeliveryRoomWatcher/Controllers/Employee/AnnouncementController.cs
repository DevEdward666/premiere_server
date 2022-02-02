using DeliveryRoomWatcher.Models.Employee;
using DeliveryRoomWatcher.Repositories.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Controllers.Employee
{
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        AnnouncementsRepo _announcement = new AnnouncementsRepo();


        [HttpPost]
        [Route("api/announcement/getAnnouncementsWebFilter")]
        public ActionResult getAnnouncementsWebFilter(AnnouncementModel.filter_announcement filter_Announcement)
        {
            return Ok(_announcement.getAnnouncementsWebFilter(filter_Announcement));
        }   
        [HttpPost]
        [Route("api/announcement/getAnnouncementsWeb")]
        public ActionResult getAnnouncementsWeb()
        {
            return Ok(_announcement.getAnnouncementsWeb());
        }  
        [HttpPost]
        [Route("api/announcement/getAnnouncements")]
        public ActionResult getAnnouncements()
        {
            return Ok(_announcement.getAnnouncements());
        }    
        [HttpPost]
        [Route("api/announcement/getAnnouncements_images")]
        public ActionResult getAnnouncements_images(AnnouncementModel.Get_Announcement_images images)
        {
            return Ok(_announcement.getAnnouncements_images(images));
        }   
        [HttpPost]
        [Route("api/announcement/InsertNewAnnouncement")]
        public ActionResult InsertNewAnnouncement([FromForm] AnnouncementModel.Create_Announcements create_Announcements)
        {
            return Ok(_announcement.InsertNewAnnouncement(create_Announcements));
        } 
        [HttpPost]
        [Route("api/announcement/UpdateAnnouncementWithImages")]
        public ActionResult UpdateAnnouncementWithImages([FromForm] AnnouncementModel.Create_Announcements create_Announcements)
        {
            return Ok(_announcement.UpdateAnnouncementWithImages(create_Announcements));
        }
        [HttpPost]
        [Route("api/announcement/AnnouncementUnpublihed")]
        public ActionResult AnnouncementUnpublihed( AnnouncementModel.Unpublished_Announcement unpublished)
        {
            return Ok(_announcement.AnnouncementUnpublihed(unpublished));
        }
        [HttpPost]
        [Route("api/announcement/AnnouncementUpdate")]
        public ActionResult AnnouncementUpdate(AnnouncementModel.update_Announcement update)
        {
            return Ok(_announcement.AnnouncementUpdate(update));
        }
    }
}
