using Dapper;
using DeliveryRoomWatcher.Config;
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


                        int insert_news= con.Execute($@"insert into prem_news (Image,Title,description,author,dateEncoded) values(@image,@title,@description,@author,now())",
                                               news, transaction: tran);
                        if (insert_news >= 0)
                        {
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
        public ResponseModel getEvents()
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        try
                        {
                            var data = con.Query($@"SELECT evid,evtitle,evdesc,evimage,DATE_FORMAT(evstartdate,'%Y-%m-%d') AS datestart,evstarttime, DATE_FORMAT(evenddate,'%Y-%m-%d') AS dateend,evendtime,evcolor,eventts,evstatus FROM prem_events",
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
