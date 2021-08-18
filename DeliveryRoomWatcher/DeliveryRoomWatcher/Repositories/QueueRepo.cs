using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class QueueRepo
    {
        public ResponseModel getuserqueuenumbers(Queue.getuserqueuenumber prem_id)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT q.queueno,q.countername,q.`status` FROM queue q JOIN prem_usermaster pu ON q.`prem_id`=pu.`prem_id` WHERE q.`prem_id`=@prem_id AND q.`status`='Queue' AND q.`countername`=@countername  AND DATE_FORMAT(docdate,'%Y-%m-%d')=DATE_FORMAT(CURDATE(),'%Y-%m-%d')",
                                                 prem_id, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel waiting(Queue.waiting waitings)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT DISTINCT MAX(q.queueno) queueno,q.countername,ql.`countername` AS counter,ql.`docdate` FROM queue q LEFT JOIN queue_log ql ON q.queueno = ql.`queueno` WHERE q.queueno IN (SELECT q.queueno FROM queue q WHERE DATE_FORMAT(q.docdate, '%Y-%m-%d') = DATE_FORMAT(CURDATE(), '%Y-%m-%d') AND ql.counter IN (SELECT ld.`countername` FROM lobbyheader lh JOIN lobbydtls ld ON lh.`lobbyno`=ld.`lobbyno`WHERE lh.`location` = 'ALL') AND q.countername=@countername GROUP BY q.countername) GROUP BY q.countername,ql.`countername`",
                                                 waitings, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getqueuemain()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT countername,countertype from queue_main where countertype='Regular'",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel generatenumberkiosk(Queue.generatecounternumber cntr)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {


                    con.Open();


                    using (var tran = con.BeginTransaction())
                    {
                        int x = 0;
             
                        string querygetmaxnumber = $@"SELECT MaxNumber FROM queue_setnumber WHERE Counter =@generated_counter";
                        string getmaxnumber = con.QuerySingle<string>(querygetmaxnumber, cntr, transaction: tran);

                        string checkifalreadygenerated= $@"SELECT queueno FROM queue WHERE prem_id =@prem_id and status='Queue'";
                        string checkifalreadygeneratedquery = con.QuerySingleOrDefault<string>(checkifalreadygenerated, cntr, transaction: tran);
         
                        if (getmaxnumber != null)
                        {
                            string query = $@"SELECT NextQueueNo(@generated_counter,'{getmaxnumber}') queueno";
                            string nextQueueNo = con.QuerySingle<string>(query, cntr, transaction: tran);
                            if (checkifalreadygeneratedquery != null)
                            {
                                string counter = checkifalreadygeneratedquery.Substring(0, 3);
                                string nextQueryCounter = nextQueueNo.Substring(0, 3);
                                if(counter!= nextQueryCounter)
                                {
                                    int isQueuegeneratednumberInserted = con.Execute($@"insert into queue set queueno='{nextQueueNo}',countername=@generated_counter,countertype=@generated_countertype,prem_id=@prem_id,phonenumber='+63',docdate=NOW(),status='Queue'",
                                                  cntr, transaction: tran);
                                    if (isQueuegeneratednumberInserted > 0)
                                    {

                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            data = nextQueueNo,
                                            success = true,
                                            message = "You can only generate queue number once per counter"
                                        };
                                    }
                                    return new ResponseModel

                                    {
                                        data = nextQueueNo,
                                        success = true,
                                        message = "You can only generate queue number once per counter"
                                    };
                                }
                                else
                                {
                                    return new ResponseModel
                                    {
                                        data = nextQueueNo,
                                        success = false,
                                        message = "You can only generate queue number once per counter"
                                    };
                                }
                            

                            }
                            else
                            {
                                int isQueuegeneratednumberInserted = con.Execute($@"insert into queue set queueno='{nextQueueNo}',countername=@generated_counter,countertype=@generated_countertype,prem_id=@prem_id,phonenumber='+63',docdate=NOW(),status='Queue'",
                                                        cntr, transaction: tran);
                                if (isQueuegeneratednumberInserted > 0)
                                {

                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        data = nextQueueNo,
                                        success = true,
                                        message = "Success! The new generated number has been added successfully"
                                    };
                                }
                                return new ResponseModel

                                {
                                    data = nextQueueNo,
                                    success = true,
                                    message = "Success! The new generated number has been added successfully"
                                };
                            }
                        
                        }
                        else
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {

                                success = false,
                                message = "Error! No rows affected while inserting the new record."
                            };
                        }

                    }
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
        public ResponseModel getcounterlist(Queue.getlobbynos counterlist)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT distinct MAX(q.queueno) queueno,q.countername,ql.`countername` AS counter,ql.`docdate` FROM queue q LEFT JOIN queue_log ql ON q.queueno = ql.`queueno` WHERE q.queueno IN (SELECT q.queueno FROM queue q WHERE DATE_FORMAT(q.docdate, '%Y-%m-%d') = DATE_FORMAT(CURDATE(), '%Y-%m-%d') AND ql.counter IN (SELECT ld.`countername` FROM lobbyheader lh JOIN lobbydtls ld ON lh.`lobbyno`=ld.`lobbyno`WHERE lh.`location` = 'ALL') GROUP BY q.countername) GROUP BY q.countername,ql.`countername`  ",
                                                 counterlist, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel generatequeuenumber(Queue.generatenumber generatenumber)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.QuerySingle($@"SELECT NextQueueNo(@counter,@maxnumber) queueno",
                                                 generatenumber, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
