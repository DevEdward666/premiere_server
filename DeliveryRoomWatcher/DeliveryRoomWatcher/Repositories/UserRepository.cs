using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.User;
using DeliveryRoomWatcher.Parameters;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
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
        CompanyRepository company = new CompanyRepository();
        public List<UserModel> authenticateUser(PAuthUser cred)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                        var user_credentials = con.Query<UserModel>($@"SELECT `username` FROM `prem_usermaster` WHERE  AES_ENCRYPT(@password,@username) = `password` and activated=true", cred, transaction: tran).ToList();

                        return user_credentials;
                      
                    
                   
                    
                }
            }
        }
        public List<UserModel> authenticateAdmin(PAuthUser cred)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                 

                    var user_credentials = con.Query<UserModel>($@"SELECT u.`username`,TRIM(p.`modid`) AS 'modid' FROM `userpermission` p
                                JOIN `usermaster` u ON u.`username` = p.`username`  
                                WHERE  p.logid = 'login' AND AES_ENCRYPT(@password,@username) = u.`password`
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
                        string emailname = user_info.firstname;
                        string emailladdress = user_info.email;
                        string query = $@"SELECT * FROM prem_usermaster WHERE username=@username";
                        var data = con.Query<string>(query, user_info, transaction: tran);
                        if (data.Count() == 0)
                        {

                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                            int insert_user_information = con.Execute($@"INSERT INTO prem_usersinfo set username=@username,firstname=@firstname,middlename=@middlename,lastname=@lastname,gender=@gender,
                                                                        birthdate=date_Format(@birthdate,'%Y-%m-%d'),mobileno=@mobileno,email=@email",
                                                                    user_info, transaction: tran);

                            if (insert_user_information >= 0)
                            {
                                int insert_user_cred = con.Execute($@"INSERT INTO prem_usermaster set fullname=CONCAT(@lastname,',',@firstname,' ',@middlename),email=@email,username=@username,
                                                                    password=AES_ENCRYPT(@password,@username),activated='N',active='false',creaateddate=NOW()",
                                                                      user_info, transaction: tran);

                                if (insert_user_cred >= 0)
                                {
                                    int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  set otp={otpnumber},toUserName=@username,date_expire=NOW(),createdAt=NOW()",
                                                                     user_info, transaction: tran);
                                    if (insert_user_otp >= 0)
                                    {
                                        var message = new MimeMessage();
                                        string hospitalame = company.CompanyName().data.ToString();
                                        message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
                                        message.To.Add(new MailboxAddress(emailname, emailladdress));
                                        message.Subject = "PSHTeleHelth OTP";

                                        message.Body = new TextPart("plain")
                                        {
                                            Text = "This is your OTP " + otpnumber + " it will expire in 300 seconds"
                                        };
                                        using (var client = new SmtpClient())
                                        {
                                            client.Connect("smtp.gmail.com", 587, false);


                                            client.Authenticate(DefaultConfig._providerEmailAddress, DefaultConfig._providerEmailPass);

                                            client.Send(message);
                                            client.Disconnect(true);
                                        }
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
                  
                            //string query2 = $@"SELECT * FROM prem_usermaster WHERE username=@username and activated='N' and active='false'";
                            //var data2 = con.Query<string>(query2, user_info, transaction: tran);
                            //if (data2.Count() == 0)
                            //{
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Wrong Usename/Password. Please Try again"
                                };
                            //}
                            //else
                            //{
                            //    return new ResponseModel
                            //    {
                            //        success = true,
                            //        message = ""
                            //    };
                            //}
                      

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
        public ResponseModel updateimg(FileModelProfile prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        int insert_user_otp = con.Execute($@"UPDATE prem_usersinfo SET img=@img WHERE username=@username",
                                                       prem, transaction: tran);
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
        public ResponseModel updatedocs( FileModel prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                              
                            
                                int insert_user_otp = con.Execute($@"UPDATE prem_usersinfo SET docs=@docs WHERE username=@username",
                                                       prem, transaction: tran);
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
        public ResponseModel getUserInfoAdmin(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.QuerySingle($@"SELECT u.empname,emp.* FROM empmast emp JOIN usermaster u ON emp.idno=u.username WHERE u.username= @username",username, transaction: tran);
                        string brand_logo = defaults.hospitalLogo().data.ToString();
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(username.username, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        var brand_logo_bitmap = UseFileParser.Base64StringToBitmap(brand_logo);
                        Bitmap qrCodeImage = qrCode.GetGraphic(35, Color.Black, Color.White, null, 10);
                        string qr_with_brand_logo = UseFileParser.BitmapToBase64(qrCodeImage);
                        return new ResponseModel
                        {
                            success = true,
                            data = data,
                            message= qr_with_brand_logo
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
                                        $@"SELECT pu.*,pum.pin,pum.isLocked,pum.active,pum.activated,pum.ApproveAt,pum.passbase_id,pum.passbase_status FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.prem_id=pum.prem_id where pu.username= @username
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
                        string getUsernametoOTP = con.QuerySingleOrDefault<string>(query, username, transaction: tran);
                        if (getUsernametoOTP != null)
                        {
                            if (getUsernametoOTP.Equals(username.otp))
                            {
                                int updateuser = con.Execute($@"UPDATE prem_usermaster SET activated='Y' WHERE username=@username",
                                                    username, transaction: tran);

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
                                                message = "OTP Expired."
                                            };
                                        }

                                    }
                                    else
                                    {
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
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "OTP Not Exist."
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
