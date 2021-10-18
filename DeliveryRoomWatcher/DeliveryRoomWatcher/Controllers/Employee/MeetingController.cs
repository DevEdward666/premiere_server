using DeliveryRoomWatcher.Repositories.Employee;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Controllers.Employee
{
    [ApiController]
    public class MeetingController : ControllerBase
    {
        MeetingRepo _meeting = new MeetingRepo();


        [HttpPost]
        [Route("api/meetings/getMeetings")]
        public ActionResult getMeetings()
        {
            return Ok(_meeting.getMeetings());
        }
    }
}
