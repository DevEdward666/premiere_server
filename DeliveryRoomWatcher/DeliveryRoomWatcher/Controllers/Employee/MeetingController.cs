using DeliveryRoomWatcher.Models.Employee;
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
        [Route("api/meetings/SelectedMeetings")]
        public ActionResult SelectedMeetings(MeetingModel.SelectedMeeting selectedmeeting)
        {
            return Ok(_meeting.SelectedMeetings(selectedmeeting));
        }   
        [HttpPost]
        [Route("api/meetings/getListDept")]
        public ActionResult getListDept()
        {
            return Ok(_meeting.getListDept());
        }  
        [HttpPost]
        [Route("api/meetings/getCustomDept")]
        public ActionResult getCustomDept()
        {
            return Ok(_meeting.getCustomDept());
        }  
        [HttpPost]
        [Route("api/meetings/getMeetings")]
        public ActionResult getMeetings(MeetingModel.MeetingList meetingList)
        {
            return Ok(_meeting.getMeetings(meetingList));
        }     
        [HttpPost]
        [Route("api/meetings/UPDATEMeeting")]
        public ActionResult UPDATEMeeting([FromForm] MeetingModel.Create_Meeting create_Meeting )
        {
            return Ok(_meeting.UPDATEMeeting(create_Meeting));
        } 
        [HttpPost]
        [Route("api/meetings/attendMeeting")]
        public ActionResult attendMeeting([FromForm] MeetingModel.Attend_Meeting attend_Meeting )
        {
            return Ok(_meeting.attendMeeting(attend_Meeting));
        }        
        [HttpPost]
        [Route("api/meetings/InsertNewMeeting")]
        public ActionResult InsertNewMeeting([FromForm] MeetingModel.Create_Meeting create_Meeting )
        {
            return Ok(_meeting.InsertNewMeeting(create_Meeting));
        }   
        [HttpPost]
        [Route("api/meetings/InsertNewMeetingDept")]
        public ActionResult InsertNewMeetingDept([FromForm] MeetingModel.Create_Meeting_Dept create_Meeting_Dept )
        {
            return Ok(_meeting.InsertNewMeetingDept(create_Meeting_Dept));
        }
    }
}
