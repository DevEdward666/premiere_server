using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Entities;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.Employee;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories.Employee
{
    public class MeetingRepo
    {
        public ResponseModel getListDept()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pmd.`meeting_dept_id`,pmd.`dept_name` FROM prem_meetings_dept pmd", null, transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }     public ResponseModel getCustomDept()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pmd.`meeting_dept_id`,pmd.`dept_name`,CONCAT(emp.`lastname`,',',emp.`firstname`,' ',emp.`middlename`)fullname FROM 
                        prem_meetings_dept pmd JOIN prem_meetings_dept_emp pmde ON pmde.`meeting_dept_id`=pmd.`meeting_dept_id` JOIN empmast emp ON emp.`idno`=pmde.`emp_id`", null, transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }   
        public ResponseModel getMeetings(MeetingModel.MeetingList meetingList)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pm.id,pm.`from`,pm.`subject`,pm.location,pm.description,DATE_FORMAT(pm.startdate,'%Y-%m-%d') startdate,pm.starttime,DATE_FORMAT(pm.enddate,'%Y-%m-%d')enddate,TIME_FORMAT(pm.endtime,'%H:%m')endtime,pm.published,pma.attended,pma.remarks from  prem_meetings pm JOIN prem_meetings_attendees pma ON pm.`id`=pma.`meeting_id` WHERE pma.`meetings_dept`=@emp_id", meetingList, transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }  
        public ResponseModel SelectedMeetings(MeetingModel.SelectedMeeting selectedmeeting)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT id,`from`,`subject`,location,description,DATE_FORMAT(startdate,'%Y-%m-%d') startdate,TIME_FORMAT(starttime,'%H:%m')starttime,
                        DATE_FORMAT(enddate,'%Y-%m-%d')enddate,TIME_FORMAT(endtime,'%H:%m')endtime,published FROM prem_meetings WHERE id=@meeting_id", selectedmeeting, transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }
        public ResponseModel attendMeeting(MeetingModel.Attend_Meeting attend_Meeting)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int insert_appointment = con.Execute($@"UPDATE prem_meetings_attendees SET attended=@attended,remarks=@remarks WHERE meeting_id=@meeting_id AND meetings_dept=@meetings_dept",
                                               attend_Meeting, transaction: tran);
                        if (insert_appointment >= 0)
                        {
                            tran.Commit();
                            if(attend_Meeting.attended=="true")
                            {
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Your meeting is added to your calendar"
                                };
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Thank you for your response."
                                };
                            }
                        }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "No affected rows when trying to save the consultation request. "
                                };
                            }



                     





                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }


                }
            }
        }    
        public ResponseModel UPDATEMeeting(MeetingModel.Create_Meeting create_Meeting)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {



                            int insert_appointment = con.Execute($@"UPDATE prem_meetings SET `from`=@from,`subject`=@subject,location=@location,description=@description,startdate=@startdate,starttime=@starttime,enddate=@enddate,endtime=@endtime WHERE id=@meeting_id",
                                               create_Meeting, transaction: tran);
                            if (insert_appointment >= 0)
                            {

                                
                         

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Meeting Successfully Updated"
                                };




                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "No affected rows when trying to save the consultation request. "
                                };
                            }



                     





                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }


                }
            }
        }     
        public ResponseModel InsertNewMeeting(MeetingModel.Create_Meeting create_Meeting)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        string MeetingId = $@"SELECT NextMeetingId()";
                        string getMeetingId = con.QuerySingleOrDefault<string>(MeetingId, create_Meeting, transaction: tran);
              
                        create_Meeting.meeting_id = getMeetingId;
                        create_Meeting.meeting_id = getMeetingId;
                        string querystringattendees = $@"SELECT emp_id FROM prem_meetings_dept_emp WHERE meeting_dept_id=@meeting_dept_id";
                        List<MeetingModel.CreateMeetingAttendees> queryattendees =  con.Query<MeetingModel.CreateMeetingAttendees>(querystringattendees, create_Meeting, transaction: tran).ToList();
                        create_Meeting.selectedattendees = queryattendees;
                        if (getMeetingId != null)
                        {

                            int insert_appointment = con.Execute($@"INSERT INTO prem_meetings SET id=@meeting_id,`From`=@from,location=@location,`Subject`=@subject,Description=@description,
                                                                    startdate=@startdate,starttime=@starttime,enddate=@enddate,endtime=@endtime,published=true",
                                               create_Meeting, transaction: tran);
                            if (insert_appointment >= 0)
                            {

                                foreach (var f in create_Meeting.attach_files)
                                {

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var meeting_file_payload = new MeetingFileEntity()
                                    {
                                        meeting_id = getMeetingId
                                    };

                                    file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources/Meetings/{create_Meeting.meeting_id}/", create_Meeting.meeting_id);
                                    if (!file_upload_response.success)
                                    {
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = file_upload_response.message
                                        };
                                    }
                                    else
                                    {
                                        meeting_file_payload.file_dest = $@"Resources/Meetings/{create_Meeting.meeting_id}/{file_upload_response.data.name}";
                                        meeting_file_payload.file_name = file_upload_response.data.name;
                                    }


                                    int add_file = con.Execute(@"INSERT INTO `prem_meetings_files` SET meeting_id=@meeting_id, file_dest=@file_dest,file_name=@file_name,createdDate=NOW();",
                                        meeting_file_payload, transaction: tran);

                                    if (add_file <= 0)
                                    {
                                        tran.Rollback();
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = $"The {f.FileName} could not be saved! Please try again!"
                                        };
                                    }
                                }
                                if (create_Meeting.selectedattendees != null)
                                {
                                    foreach (var attendees in create_Meeting.selectedattendees)
                                    {
                                        int i = create_Meeting.selectedattendees.Count;
                                        int x = 0;
                                        int insert_department = con.Execute($@"INSERT INTO prem_meetings_attendees SET meeting_id='{getMeetingId}',meetings_dept=@emp_id,createdDate=NOW()", attendees, transaction: tran);
                                        if (insert_department <= 0)
                                        {
                                            tran.Rollback();
                                            response.success = false;
                                            response.message = "Error! Insertion of Log Failed.";

                                        }

                                    }
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Meeting Successfully Published"
                                    };


                                }

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Meeting Successfully Published"
                                };




                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "No affected rows when trying to save the consultation request. "
                                };
                            }



                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "No affected rows when trying to save the consultation request. "
                            };
                        }





                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }


                }
            }
        }
        public ResponseModel InsertNewMeetingDept(MeetingModel.Create_Meeting_Dept create_Meeting_Dept)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        string MeetingId = $@"SELECT NextMeetingDeptId()";
                        string getMeetingId = con.QuerySingleOrDefault<string>(MeetingId, create_Meeting_Dept, transaction: tran);
                        create_Meeting_Dept.meeting_dept_id = getMeetingId;
                        create_Meeting_Dept.meeting_dept_id = getMeetingId;

                        if (getMeetingId != null)
                        {

                            int insert_appointment = con.Execute($@"INSERT INTO prem_meetings_dept SET meeting_dept_id=@meeting_dept_id,dept_name=@dept_name,notes=@notes,createdDate=NOW()",
                                               create_Meeting_Dept, transaction: tran);
                            if (insert_appointment >= 0)
                            {

                             
                                if (create_Meeting_Dept.selectedattendees != null)
                                {
                                    foreach (var attendees in create_Meeting_Dept.selectedattendees)
                                    {
                                        int i = create_Meeting_Dept.selectedattendees.Count;
                                        int x = 0;
                                        int insert_department = con.Execute($@"INSERT INTO prem_meetings_dept_emp SET meeting_dept_id='{getMeetingId}',emp_id=@emp_id,createdDate=NOW()", attendees, transaction: tran);
                                        if (insert_department <= 0)
                                        {
                                            tran.Rollback();
                                            response.success = false;
                                            response.message = "Error! Insertion of Log Failed.";

                                        }

                                    }
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "New Department Added Successfully"
                                    };


                                }

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "New Department Added Successfully"
                                };




                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "No affected rows when trying to save the consultation request. "
                                };
                            }



                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "No affected rows when trying to save the consultation request. "
                            };
                        }





                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }


                }
            }
        }
    }
}
