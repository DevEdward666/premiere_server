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
        [Route("api/announcement/getAnnouncements")]
        public ActionResult getAnnouncements()
        {
            return Ok(_announcement.getAnnouncements());
        }
    }
}
