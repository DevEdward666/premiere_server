using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Entities;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class ManageRepo
    {
        public ResponseModel InsertNewNews(setNewImage news)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string NewsID = $@"SELECT NextNewsId()";
                        string getNewsID = con.QuerySingleOrDefault<string>(NewsID, news, transaction: tran);
                        news.news_id = getNewsID;
                        int insert_news= con.Execute($@"insert into prem_news set id=@news_id,Title=@title,description=@description,author=@author,dateEncoded=now()",
                                               news, transaction: tran);
                        if (insert_news >= 0)
                        {
                            foreach (var f in news.news_image)
                            {

                                FileResponseModel file_upload_response = new FileResponseModel
                                {
                                    success = true
                                };

                                var proc_file_payload = new NewsFileEntity()
                                {
                                    news_id = getNewsID
                                };

                                file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources\News\{news.news_id}\", news.title); 
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
                                    proc_file_payload.news_image = file_upload_response.data.path;
                                    proc_file_payload.news_image_name = file_upload_response.data.name;
                                }


                                int add_file = con.Execute(@"
                                INSERT INTO `prem_news_images` 
                                SET 
                                news_id=@news_id, 
                                news_image=@news_image,
                                news_image_name=@news_image_name,
                                dateEncoded=NOW();",
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
                                message = "News inserted sucessfully."
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! news insertion Failed."
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
        public ResponseModel InsertNewEvents(PEvents.PEvent news)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        int insert_new_events = con.Execute($@"INSERT INTO prem_events  SET evtitle=@evtitle,evdesc=@evdesc,evimage=@evimage,evstartdate=@evstartdate,evstarttime=@evstarttime,evenddate=@evenddate,evendtime=@evendtime,evcolor=@evcolor,evstatus='P'",
                                               news, transaction: tran);
                        if (insert_new_events >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "The events inserted sucessfully."
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! events insertion Failed."
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
        public ResponseModel InsertMasterFiles (Models.FileModel file)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string FileId = $@"SELECT NextFileId()";
                        string getFileId = con.QuerySingleOrDefault<string>(FileId, file, transaction: tran);
                        file.fileid = getFileId;
                        file.fileid = getFileId;

                        if (getFileId != null)
                        {


                            int insert_new_master_file = con.Execute($@"INSERT INTO prem_emp_file_mast SET master_file_id=@file_id, title=@title,description=@description,createdDate=NOW()",
                                               file, transaction: tran);
                        if (insert_new_master_file >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Files inserted sucessfully."
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! events insertion Failed."
                            };
                        }
                        }
                        else
                        {

                            tran.Rollback();
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
        public ResponseModel InsertFiles (Models.FileModel file)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                       
                            int insert_new_master_file = con.Execute($@"INSERT INTO prem_emp_files SET master_file_id=@file_id, filename=@filename,filepath=@filepath,filetype=@filetype,createdDate=NOW()",
                                               file, transaction: tran);
                            if (insert_new_master_file >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Files inserted sucessfully."
                                };

                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! events insertion Failed."
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
        public ResponseModel UpdateEvents(PEvents.PEvent news)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        int insert_new_events = con.Execute($@"update prem_events set evtitle=@evtitle,evdesc=@evdesc,evstartdate=@evstartdate,evstarttime=@evstarttime,evenddate=@evenddate,evendtime=@evendtime,evcolor=@evcolor where evid=@evid",
                                               news, transaction: tran);
                        if (insert_new_events >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "The events updated sucessfully."
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! events insertion Failed."
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
        public ResponseModel getFiles(Models.FileModel.Search_File search_File)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pem.master_file_id,pem.title,pem.description,pe.filename,pe.filepath 
                                                FROM prem_emp_file_mast pem JOIN prem_emp_files pe ON pem.master_file_id=pe.master_file_id
                                                WHERE pem.`title`LIKE CONCAT(@title,'%') LIMIT 50",
                           search_File, transaction: tran
                            );

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
        public ResponseModel getEventsAdmin()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT evid,evtitle,evdesc,evimage,DATE_FORMAT(evstartdate,'%Y-%m-%d') AS datestart,evstarttime, DATE_FORMAT(evenddate,'%Y-%m-%d') AS dateend,evendtime,evcolor,eventts,evstatus FROM prem_events ",
                           null, transaction: tran
                            );

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
        public ResponseModel UpdateTestimonials(Models.TestimonialsModel.update_testimonials update_testimonials)
        {
                                                                                             
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                            int insert_testimonials = con.Execute($@"UPDATE prem_testimonials SET description=@description,author=@author WHERE id=@id",
                                update_testimonials, transaction: tran);

                            if (insert_testimonials > 0)
                            {
                               
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Testimonials updated successfully"
                                    };
                             

                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = $"No affected rows when trying to save the consultation request."
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
        public ResponseModel InsertNewTestimonials(Models.TestimonialsModel.create_testimonials create_Testimonials)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                            int insert_testimonials = con.Execute($@"INSERT INTO prem_testimonials SET description=@description,author=@author,encoded=NOW()",
                                create_Testimonials, transaction: tran);

                            if (insert_testimonials > 0)
                            {
                               
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Testimonials added successfully"
                                    };
                             

                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = $"No affected rows when trying to save the consultation request."
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
            public ResponseModel InsertNewFile(Models.FileModel.Create_New_File create_file)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        string file_id = $@"SELECT NextFileId()";
                        string getfile_id = con.QuerySingleOrDefault<string>(file_id, create_file, transaction: tran);
                        create_file.master_file_id = getfile_id;
                        create_file.master_file_id = getfile_id;

                        if (getfile_id != null)
                        {
                                int add_file_mast = con.Execute($@"INSERT INTO prem_emp_file_mast SET master_file_id=@master_file_id,title=@title,description=@description,createdDate=NOW()",
                                    create_file, transaction: tran);

                            if (add_file_mast > 0)
                            {
                                int add_file = con.Execute($@"INSERT INTO prem_emp_files SET master_file_id=@master_file_id,filename=@title,filepath=@filepath,filetype=@filetype,createdAt=NOW()",
                                    create_file, transaction: tran);
                                if (add_file > 0) 
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Video added successfully"
                                    };
                                }
                                else
                                {
                                    tran.Rollback();
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = $"No affected rows when trying to save the consultation request."
                                    };
                                }

                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = $"No affected rows when trying to save the consultation request."
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
        }public ResponseModel UpdateFile(Models.FileModel.Create_New_File create_file)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                           int add_file_mast = con.Execute($@"update prem_emp_file_mast SET title=@title,description=@description where master_file_id=@master_file_id",
                                    create_file, transaction: tran);

                            if (add_file_mast > 0)
                            {
                                int add_file = con.Execute($@"update prem_emp_files SET filename=@title,filepath=@filepath  where master_file_id=@master_file_id",
                                    create_file, transaction: tran);
                                if (add_file > 0) 
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Video added successfully"
                                    };
                                }
                                else
                                {
                                    tran.Rollback();
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = $"No affected rows when trying to save the consultation request."
                                    };
                                }

                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = $"No affected rows when trying to save the consultation request."
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
        public ResponseModel getEvents()
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        try
                        {
                            var data = con.Query($@"SELECT evid,evtitle,evdesc,evimage,DATE_FORMAT(evstartdate,'%Y-%m-%d') AS datestart,evstarttime, DATE_FORMAT(evenddate,'%Y-%m-%d') AS dateend,evendtime,evcolor,eventts,evstatus FROM prem_events  ORDER BY datestart DESC",
                               null, transaction: tran
                                );

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
        public ResponseModel getEventsBymonth(PEvents.PEventByMonth month)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_events WHERE YEAR(DATE(evstartdate))=@year AND MONTH(DATE(evstartdate))=@month",
                           month, transaction: tran
                            );

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
        public ResponseModel getEventsthisweek()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_events   WHERE  evstartdate >= CURDATE() - INTERVAL DAYOFWEEK(CURDATE())+6 DAY ",
                            transaction: tran
                            );

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
        public ResponseModel getEventsthisday()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_events WHERE evstartdate=CURDATE()",
                            transaction: tran
                            );

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
        public ResponseModel getappdefaults()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_admin_apps",null,transaction: tran);

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
        public ResponseModel getEventInfo(PEvents.PGetEvent events)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT evid,evtitle,evdesc,evimage,DATE_FORMAT(evstartdate,'%m/%d/%Y') AS datestart,evstarttime, DATE_FORMAT(evenddate,'%m/%d/%Y') AS dateend,evendtime,evcolor,eventts,evstatus FROM prem_events where evid=@evid",
                           events,transaction: tran
                            );

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

        

    }
}
