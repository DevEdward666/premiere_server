using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Employee
{
    public class MeetingModel
    {
        public class Create_Meeting
        {
            public string meeting_id { get; set; }
            public string from { get; set; }
            public string subject { get; set; }
            public string description { get; set; }
            public string location { get; set; }
            public string startdate { get; set; }
            public string starttime { get; set; }
            public string enddate { get; set; }
            public string endtime { get; set; }
            public string published { get; set; }
            public string meeting_dept_id { get; set; }
            public List<IFormFile> attach_files { set; get; }
            public List<CreateMeetingAttendees> selectedattendees { get; set; }
        }     
        public class Attend_Meeting
        {
            public string meeting_id { get; set; }
            public string attended { get; set; }
            public string remarks { get; set; }
            public string meetings_dept { get; set; }
        }
        public class Create_Meeting_Dept
        {
            public string meeting_dept_id { get; set; }
            public string dept_name { get; set; }
            public string notes { get; set; }
            public List<CreateDeptAttendees> selectedattendees { get; set; }
        }
        public class SelectedMeeting
        {
            public string meeting_id { get; set; }
        }
        public class MeetingList
        {
            public string emp_id { get; set; }
        }
        public class CreateMeetingAttendees
        {
            public string emp_id { get; set; }
        }
        public class CreateDeptAttendees
        {
            public string emp_id { get; set; }
        }
    }
}
