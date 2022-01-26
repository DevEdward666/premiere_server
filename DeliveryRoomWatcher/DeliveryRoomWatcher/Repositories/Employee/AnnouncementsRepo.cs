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
    public class AnnouncementsRepo
    {
        public ResponseModel getAnnouncements()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT DISTINCT pa.id,paf.file_dest AS img,pa.Subject,pa.Description,pa.published,pa.createdDate FROM prem_announcements pa LEFT JOIN prem_announcement_file paf ON pa.id=paf.announcementId where pa.published='true' group by pa.id", null, transaction: tran);

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
        public ResponseModel getAnnouncementsWeb()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT DISTINCT pa.id,paf.file_dest AS img,pa.Subject,pa.Description,pa.published,pa.createdDate FROM prem_announcements pa LEFT JOIN prem_announcement_file paf ON pa.id=paf.announcementId  group by pa.id", null, transaction: tran);

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
        public ResponseModel getAnnouncementsWebFilter(AnnouncementModel.filter_announcement filter_Announcement)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@" SELECT DISTINCT pa.id,paf.file_dest AS img,pa.Subject,pa.Description,pa.published,pa.createdDate FROM prem_announcements pa LEFT JOIN prem_announcement_file paf ON pa.id=paf.announcementId WHERE pa.`published`LIKE  concat(@published,'%') OR MONTH(pa.`createdDate`) like concat('%',@month,'%') AND YEAR( pa.`createdDate`)like concat ('%',@year,'%')  GROUP BY pa.id", filter_Announcement, transaction: tran);

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
        public ResponseModel getAnnouncements_images(AnnouncementModel.Get_Announcement_images images)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  paf.file_dest AS img FROM prem_announcement_file paf WHERE paf.announcementId=@announcement_id", images, transaction: tran);

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
        public ResponseModel AnnouncementUnpublihed(AnnouncementModel.Unpublished_Announcement unpublished)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int update_unpublished = con.Execute($@"UPDATE prem_announcements SET published='false' WHERE id=@announcement_id", unpublished, transaction: tran);

                      
                        if(update_unpublished >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Announcement Unpublished"
                            };
                        }
                        else
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Something Went Wrong"
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
        public ResponseModel AnnouncementUnpublished(AnnouncementModel.update_Announcement update)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int update_unpublished = con.Execute($@"UPDATE prem_announcements SET published=@published WHERE id=@announcement_id", update, transaction: tran);


                        if (update_unpublished >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Announcement Successfully Updated"
                            };
                        }
                        else
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Something Went Wrong"
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
        public ResponseModel AnnouncementUpdate(AnnouncementModel.update_Announcement update)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int update_unpublished = con.Execute($@"UPDATE prem_announcements SET Subject=@subject,Description=@description WHERE id=@announcement_id", update, transaction: tran);


                        if (update_unpublished >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Announcement Successfully Updated"
                            };
                        }
                        else
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Something Went Wrong"
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
        public ResponseModel UpdateAnnouncementWithImages(AnnouncementModel.Create_Announcements create_announcement)
        {
            ResponseModel response = new ResponseModel();
            ImagePathtoFormFile path = new ImagePathtoFormFile();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {



                            int insert_appointment = con.Execute($@"UPDATE prem_announcements SET Subject=@subject,Description=@description WHERE id=@announcement_id",
                                               create_announcement, transaction: tran);
                        int delete_announcement = con.Execute($@"DELETE from prem_announcement_file where announcementId=@announcement_id ",
                                               create_announcement, transaction: tran);
                            if (insert_appointment >= 0 && delete_announcement>=0)
                            {
                                foreach (var f in create_announcement.attach_files)
                                {

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new AnnouncementFileEntity()
                                    {
                                        announcementId = create_announcement.announcement_id
                                    };

                                    file_upload_response = UseLocalFiles.UploadLocalFile(f,$@"Resources/Announcements/{create_announcement.announcement_id}/", create_announcement.announcement_id);
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
                                        proc_file_payload.file_dest = $@"Resources/Announcements/{create_announcement.announcement_id}/{file_upload_response.data.name}";
                                        proc_file_payload.file_name = file_upload_response.data.name;
                                    }


                                    int add_file = con.Execute(@"INSERT INTO `prem_announcement_file` SET announcementId=@announcementId, file_dest=@file_dest,file_name=@file_name,encoded_at=NOW();",
                                        proc_file_payload, transaction: tran);

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
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Announcement Successfully Updated"
                            };

                        }


                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Announcement Successfully Updated"
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
        public ResponseModel InsertNewAnnouncement(AnnouncementModel.Create_Announcements create_announcement)
        {
            ResponseModel response = new ResponseModel();
            ImagePathtoFormFile path = new ImagePathtoFormFile();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        string AnnouncementId = $@"SELECT NextAnnouncementId()";
                        string getAnnouncementId = con.QuerySingleOrDefault<string>(AnnouncementId, create_announcement, transaction: tran);
                        create_announcement.announcement_id = getAnnouncementId;
                        create_announcement.announcement_id = getAnnouncementId;

                        if (getAnnouncementId != null)
                        {

                            int insert_appointment = con.Execute($@"INSERT INTO prem_announcements SET id=@announcement_id,Subject=@subject,Description=@description,published=@published,createdDate=NOW()",
                                               create_announcement, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                                foreach (var f in create_announcement.attach_files)
                                {

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new AnnouncementFileEntity()
                                    {
                                        announcementId = getAnnouncementId
                                    };

                                    file_upload_response = UseLocalFiles.UploadLocalFile(f,$@"Resources/Announcements/{create_announcement.announcement_id}/", create_announcement.announcement_id);
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
                                        proc_file_payload.file_dest = $@"Resources/Announcements/{create_announcement.announcement_id}/{file_upload_response.data.name}";
                                        proc_file_payload.file_name = file_upload_response.data.name;
                                    }


                                    int add_file = con.Execute(@"INSERT INTO `prem_announcement_file` SET announcementId=@announcementId, file_dest=@file_dest,file_name=@file_name,encoded_at=NOW();",
                                        proc_file_payload, transaction: tran);

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

                              
                                }


                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Announcement Successfully Published"
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
    }
}
