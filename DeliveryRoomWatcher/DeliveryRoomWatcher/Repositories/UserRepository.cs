using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.User;
using DeliveryRoomWatcher.Parameters;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DeliveryRoomWatcher.Repositories
{
    public class UserRepository
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        DefaultsRepo defaults = new DefaultsRepo();
        public List<UserModel> authenticateUser(PAuthUser cred)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                        var user_credentials = con.Query<UserModel>($@"SELECT `username` FROM `prem_usermaster` WHERE  AES_ENCRYPT(@password,@username) = `password` and active='true' and activated='Y'", cred, transaction: tran).ToList();

                        return user_credentials;
                      
                    
                   
                    
                }
            }
        }
        public List<UserModel> authenticateAdmin(PAuthUser cred)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    var user_credentials = con.Query<UserModel>($@"SELECT u.`username`,TRIM(p.`modid`) AS 'modid' FROM `userpermission` p
                                JOIN `usermaster` u ON u.`username` = p.`username`  
                                WHERE p.`modid` = 'user' AND p.logid = 'login'  OR p.`modid` = 'admin'  AND p.logid = 'login' AND AES_ENCRYPT(@password,@username) = u.`password`
                                GROUP BY p.modid", cred, transaction: tran).ToList();
                    return user_credentials;




                }
            }
        } 
 
        public ResponseModel InserNewUser(PAddNewUsers user_info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try {
                        string query = $@"SELECT * FROM prem_usermaster WHERE username=@username";
                        var data = con.Query<string>(query, user_info, transaction: tran);
                        if (data.Count() == 0)
                        {

                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                            int insert_user_information = con.Execute($@"INSERT INTO prem_usersinfo (idno,img,docs,username,firstname,middlename,lastname,gender,
                                                                    birthdate,mobileno,email,region_code,city_code,province_code,barangay_code,zipcode,nationality_code,address)
                                                                    VALUES(null,@url,@url_docs,@username,@firstname,@middlename,@lastname,@gender,
                                                                    date_Format(@birthdate,'%Y-%m-%d'),@mobileno,@email,@region_code,@city_code,
                                                                    @province_code,@barangay_code,@zipcode,@nationality_code,@fulladdress)",
                                                                    user_info, transaction: tran);

                            if (insert_user_information >= 0)
                            {
                                int insert_user_cred = con.Execute($@"INSERT INTO prem_usermaster(fullname,email,username,PASSWORD,pin,activated,active,creaateddate)
                                                                    VALUES(CONCAT(@lastname,',',@firstname,' ',@middlename),@email,@username,
                                                                    AES_ENCRYPT(@password,@username),@pin,'N','false',NOW())",
                                                                      user_info, transaction: tran);

                                if (insert_user_cred >= 0)
                                {
                                    int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  (id,otp,toUserName,date_expire,createdAt) VALUES(NULL,{otpnumber},@username,NOW(),NOW())",
                                                                     user_info, transaction: tran);
                                    if (insert_user_otp >= 0)
                                    {
                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            success = true,
                                            message = "The User credentials has been Added sucessfully.",
                                            other_info= otpnumber

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
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = "Error! Insert OTP Failed."
                                    };
                                }

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Insert Failed."
                                };

                            }
                        }
                        else{
                            return new ResponseModel
                            {
                                success = false,
                                message = "User Already Exist"
                            };
                        }
                    }
                    catch (Exception e) {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }
                  
                }
            }
        }
        public ResponseModel InserNewOTP(PAddNewOTP user_info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string query = $@"SELECT * FROM prem_usermaster WHERE username=@username and activated='N'";
                        var data = con.Query<string>(query, user_info, transaction: tran);
                        if (data.Count() != 0)
                        {
                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                                int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  (id,otp,toUserName,date_expire,createdAt) VALUES(NULL,{otpnumber},@username,NOW(),NOW())",
                                                       user_info, transaction: tran);
                                if (insert_user_otp >= 0)
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "The User credentials has been Added sucessfully.",
                                        other_info=otpnumber
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
                            string query2 = $@"SELECT * FROM prem_usermaster WHERE username=@username and activated='Y' and active='N'";
                            var data2 = con.Query<string>(query2, user_info, transaction: tran);
                            if (data2.Count() == 0)
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Your Account is Still on Review. We Will Email You Once it's Verified"
                                };
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Your Account is Still on Review. We Will Email You Once it's Verified"
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
        public ResponseModel updatelockeduser(mdlLocked username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                    
                            
                                int insert_user_otp = con.Execute($@"UPDATE prem_usermaster SET isLocked=@islocked WHERE username=@username",
                                                       username, transaction: tran);
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
        public ResponseModel getUserInfo(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
    
                        var data = con.QuerySingle(
                                        $@"SELECT pu.*,pum.pin,pum.isLocked FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.prem_id=pum.prem_id where pu.username= @username
                                        ",
                          username, transaction: tran
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
        public ResponseModel getuserpin(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.QuerySingleOrDefault($@"SELECT pin FROM prem_usermaster WHERE username=@username", username, transaction: tran);
                
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
        public ResponseModel getUserMobile(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.QuerySingle(
                                        $@"SELECT mobileno FROM prem_usersinfo where username= @username
                                        ",
                          username, transaction: tran
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
        public ResponseModel getUsernameExist(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.QuerySingle($@"select username from prem_usermaster where username=@username",
                            username, transaction: tran
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
        public ResponseModel getusersqr(string username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                  
                        string brand_logo = defaults.hospitalLogo().data.ToString();
                        var data = con.QuerySingle($@"select prem_id from prem_usermaster where prem_id=@username",
                             new { username }, transaction: tran
                            );
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(username, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);


                        var brand_logo_bitmap = UseFileParser.Base64StringToBitmap(brand_logo);
                        Bitmap qrCodeImage = qrCode.GetGraphic(35, Color.Black, Color.White, brand_logo_bitmap, 25);
                        string qr_with_brand_logo = UseFileParser.BitmapToBase64(qrCodeImage);

                        return new ResponseModel
                        {
                            success = true,
                            data = qr_with_brand_logo
                        };
                    }
              

                }
            

        }
        public ResponseModel getUserOTP(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                      
                        string query = $@"SELECT otp FROM prem_otp WHERE toUserName=@username and TIMESTAMPDIFF(SECOND,date_expire,NOW())<=300 Limit 1";
                        var getUsernametoOTP = con.QuerySingleOrDefault(query,  username , transaction: tran);
                        if (getUsernametoOTP != null)
                        {
                            int updateuser = con.Execute($@"UPDATE prem_usermaster SET activated='Y' WHERE username=@username",
                                                    username , transaction: tran);

                            if (updateuser == 1)
                            {

                                string userappid = $@"SELECT NextPremiereAPPID(@username)";
                                string getuserappid = con.QuerySingleOrDefault<string>(userappid, username, transaction: tran);
                                if (getuserappid != null)
                                {
                                    int updateumappid = con.Execute($@"UPDATE prem_usermaster SET prem_id='{getuserappid}' WHERE username=@username",
                                                   username, transaction: tran);

                                    if (updateumappid == 1)
                                    {
                                        int updateuiappid = con.Execute($@"UPDATE prem_usersinfo SET prem_id='{getuserappid}' WHERE username=@username",
                                                 username, transaction: tran);

                                        if (updateuiappid == 1)
                                        {
                                            tran.Commit();
                                            return new ResponseModel
                                            {
                                                success = true,
                                                message = "Registration Complete. Your account is still on review, We will email you once it's Verified. Thank You"
                                            };
                                        }
                                        else {
                                            return new ResponseModel
                                            {
                                                success = false,
                                                message = "OTP Expired."
                                            };
                                        }
                                           
                                    }
                                    else
                                    {
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = "OTP Expired."
                                        };
                                    }

                                }
                                else {
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = "User not exist"
                                    };
                                }

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "OTP Expired."
                                };
                            }
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Try again later."
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
