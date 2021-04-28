using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.User;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class MessagesRepo
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        public ResponseModel getmessages(SignalR_userMessageDetails.MessageDetail messageDetail)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM(SELECT * FROM prem_messages WHERE `from` =@sender AND `to` = @receiver OR `from` = @receiver AND `to` = @sender ORDER BY id DESC LIMIT @offset) sub ORDER BY id ASC ", messageDetail,
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
        public ResponseModel getusers(SignalR_userMessageDetails.MessageUsers messageDetail)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pu.prem_id,pu.fullname,pm.img,CASE WHEN(SELECT  message FROM prem_messages WHERE `from`= @receiver AND `to`=pu.prem_id OR `from`=pu.prem_id AND `to`=@receiver  ORDER BY sendAt DESC LIMIT 1)
                            IS NULL THEN 'Send them your first message' ELSE(SELECT  message FROM prem_messages WHERE `from`= @receiver AND `to`= pu.prem_id OR `from`= pu.prem_id AND `to`= @receiver  ORDER BY sendAt DESC LIMIT 1)
                            END AS last_message FROM prem_usermaster pu JOIN prem_usersinfo pm  ON pu.prem_id = pm.prem_id WHERE pu.active = TRUE", messageDetail,
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
        public ResponseModel sendmessage(SignalR_userMessageDetails.MessageDetail messageDetail)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                      

                            int send_message = con.Execute($@"insert into prem_messages (message,`from`,`to`) values(@message,@receiver,@sender)",
                                                   messageDetail, transaction: tran);
                            if (send_message >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Message Sent "
                                };

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Link user Failed."
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
