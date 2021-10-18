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
                            foreach (var f in news.image)
                            {

                                FileResponseModel file_upload_response = new FileResponseModel
                                {
                                    success = true
                                };

                                var proc_file_payload = new NewsFileEntity()
                                {
                                    news_id = getNewsID
                                };

                                file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources\News\{news.news_id}\", news.title); ;
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
                                message = "The news inserted sucessfully."
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


                        int insert_new_events = con.Execute($@"insert into prem_events (evtitle,evdesc,evimage,evstartdate,evstarttime,evenddate,evendtime,evcolor) values(@evtitle,@evdesc,@evimage,@evstartdate,@evstarttime,@evenddate,@evendtime,@evcolor)",
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
        public ResponseModel getEvents()
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        try
                        {
                            var data = con.Query($@"SELECT evid,evtitle,evdesc,evimage,DATE_FORMAT(evstartdate,'%Y-%m-%d') AS datestart,evstarttime, DATE_FORMAT(evenddate,'%Y-%m-%d') AS dateend,evendtime,evcolor,eventts,evstatus FROM prem_events  WHERE  MONTH(evstartdate) =  MONTH(CURDATE()) ORDER BY datestart",
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
