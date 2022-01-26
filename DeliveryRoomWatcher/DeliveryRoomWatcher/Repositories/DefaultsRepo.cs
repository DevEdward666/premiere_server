using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.FCM;
using DeliveryRoomWatcher.Models.User;
using DeliveryRoomWatcher.Parameters;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class DefaultsRepo
    {
           public ResponseModel GetHospitalName()
        {
            try
            {
                using MySqlConnection con = new MySqlConnection(DatabaseConfig.GetConnection());
                con.Open();
                using var tran = con.BeginTransaction();
                var data = con.QuerySingle<string>($@"SELECT val FROM `def_val` WHERE remarks = 'hosp_name'"
                                 , null, transaction: tran);
                return new ResponseModel
                {
                    success = true,
                    data = data
                };

            }
            catch (Exception err)
            {

                return new ResponseModel
                {
                    success = false,
                    message = err.Message
                };
            }
        }

        public ResponseModel GetHospitalInitial()
        {
            try
            {
                using MySqlConnection con = new MySqlConnection(DatabaseConfig.GetConnection());
                con.Open();
                using var tran = con.BeginTransaction();
                var data = con.QuerySingle<string>($@"SELECT val FROM `def_val` WHERE remarks = 'hosp_initial'"
                                 , null, transaction: tran);
                return new ResponseModel
                {
                    success = true,
                    data = data
                };

            }
            catch (Exception err)
            {

                return new ResponseModel
                {
                    success = false,
                    message = err.Message
                };
            }
        }

        public ResponseModel gettestimonials()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_testimonials", transaction: tran);

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
        public ResponseModel getRegion()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM region", transaction: tran);

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
        public ResponseModel hospitalLogo()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        string logo = Convert.ToBase64String(con.QuerySingle<byte[]>($@"
                                        SELECT hosplogo  FROM hospitallogo WHERE hospcode = GetDefaultValue('hospinitial')
                                        "
                                         , null, transaction: tran));
                        return new ResponseModel
                        {
                            success = true,
                            data = logo
                        };
                    }
                }

            }
            catch (Exception err)
            {

                return new ResponseModel
                {
                    success = false,
                    message = err.Message
                };
            }
        }

        public ResponseModel getCity(PDefaults def)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM citymunicipality WHERE regioncode=@region_code AND provincecode=@province_code", def, transaction: tran);

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
        public ResponseModel gettokens()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT token FROM prem_fcm_user_token", null, transaction: tran);

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
        public ResponseModel getSpecificToken(UserModel.getoken token)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT token FROM prem_fcm_user_token WHERE user_id=@user_id", token, transaction: tran);

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
        public ResponseModel getSpecificTokenAdmin(UserModel.getoken token)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT token FROM prem_fcm_user_token WHERE user_id=@user_id", token, transaction: tran);

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
        public ResponseModel getNationality()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM nationality", transaction: tran);

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

        public ResponseModel getProvince(PDefaults def)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM province WHERE regioncode=@region_code ",def, transaction: tran);

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
        public ResponseModel getBarangay(PDefaults def)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM barangay WHERE regioncode=@region_code AND provincecode=@province_code AND citymuncode=@city_code", def,transaction: tran);

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
        public ResponseModel getCivilStatus()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM ddt_civilstatus", transaction: tran);

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
        public ResponseModel getDepartments()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_dept", transaction: tran);

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
        public ResponseModel getReligion()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM ddt_religion", transaction: tran);

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
        public ResponseModel getProcedures(PDIagnostics.SearchProcedure search)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM ddt_prochdr where procdesc LIKE CONCAT('%',@procedure,'%') limit 10", search, transaction: tran);

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
        public ResponseModel getnotications(mdlNotifications notifications)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"  
  SELECT id,title,body,priority,audience, CASE WHEN (SELECT fullname FROM prem_usermaster WHERE prem_id=createdBy) IS NOT NULL THEN (SELECT fullname FROM prem_usermaster WHERE prem_id=createdBy) 
  WHEN (SELECT fullname FROM prem_usermaster WHERE prem_id=createdBy) IS NULL THEN 
  (SELECT empname FROM usermaster WHERE username=createdBy)  END AS createdBy,
  published,createadAt FROM prem_notifications WHERE audience='all' || audience= @name ORDER BY createadAt DESC LIMIT @offset", notifications, transaction: tran);

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
        public ResponseModel getnoticationsAll(mdlNotifications notifications)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"  
  SELECT id,title,body,priority,audience, CASE WHEN (SELECT fullname FROM prem_usermaster WHERE prem_id=createdBy) IS NOT NULL THEN (SELECT fullname FROM prem_usermaster WHERE prem_id=createdBy) 
  WHEN (SELECT fullname FROM prem_usermaster WHERE prem_id=createdBy) IS NULL THEN 
  SELECT empname FROM usermaster WHERE username=createdBy)  END AS createdBy,
  published,createadAt FROM prem_notifications WHERE audience='all' ORDER BY createadAt desc LIMIT @offset", notifications, transaction: tran);

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
        public ResponseModel getnoticationsAdmin(mdlNotifications notifications)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_notifications", notifications, transaction: tran);

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
        public ResponseModel getnoticationsAdmin(mdlNotifications.searchNotif notifications)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_notifications where title LIKE concat('%',@title,'%') or priority=@priority ORDER BY createadAt DESC LIMIT @offset  ", notifications, transaction: tran);

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
        public ResponseModel insertNotifications(mdlNotifications.createnotifications notifications)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        int insert_user_otp = con.Execute($@"INSERT INTO prem_notifications (title,body,priority,audience,createdBy,createadAt) values(@title,@body,@priority,@audience,@created_by,NOW()) ",
                                               notifications, transaction: tran);
                        if (insert_user_otp >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Updated"
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! Insert usermaster Failed."
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
        public ResponseModel insertFCMToken(NotificationModel.inserttoken inserttoken)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string settoken = $@"SELECT token FROM prem_fcm_user_token WHERE user_id=@user_id";
                        string gettoken = con.QuerySingleOrDefault<string>(settoken, inserttoken, transaction: tran);
                      
                        string setmodel = $@"SELECT phone_model FROM prem_fcm_user_token WHERE user_id=@user_id";
                        string getmodel = con.QuerySingleOrDefault<string>(setmodel, inserttoken, transaction: tran);
                    
                        if (gettoken == null)
                        {
                            int insert_new_token = con.Execute($@"INSERT INTO prem_fcm_user_token SET user_id=@user_id,token=@token,phone_brand=@phone_brand,phone_model=@phone_model,createdAt=NOW()",
                                               inserttoken, transaction: tran);
                            if (insert_new_token >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Updated"
                                };

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Insert usermaster Failed."
                                };
                            }

                        }
                        else
                        {
                           
                            if (gettoken!= inserttoken.token && getmodel==inserttoken.phone_model)
                            {
                                int update_new_token = con.Execute($@"update prem_fcm_user_token SET token=@token,phone_brand=@phone_brand,phone_model=@phone_model,createdAt=NOW() where user_id=@user_id",
                                              inserttoken, transaction: tran);
                                if (update_new_token >= 0)
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Updated"
                                    };

                                }
                                else
                                {
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = "Error! Insert usermaster Failed."
                                    };
                                }
                            }
                            else if(gettoken != inserttoken.token && getmodel != inserttoken.phone_model)
                            {
                                int insert_new_token_from_other_device = con.Execute($@"INSERT INTO prem_fcm_user_token SET user_id=@user_id,token=@token,phone_brand=@phone_brand,phone_model=@phone_model,createdAt=NOW()",
                                          inserttoken, transaction: tran);
                                if (insert_new_token_from_other_device >= 0)
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Updated"
                                    };

                                }
                                else
                                {
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = "Error! Insert usermaster Failed."
                                    };
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "No tokens need to update"
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
    }
}
