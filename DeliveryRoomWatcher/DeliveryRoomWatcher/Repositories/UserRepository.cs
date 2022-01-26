using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Entities;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.Doctors;
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
using static DeliveryRoomWatcher.Models.User.UserModel;

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
        public List<UserModel> authenticateDoctor(PAuthUser cred)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                        var user_credentials = con.Query<UserModel>($@"SELECT `username` FROM `prem_doctor_usermaster` WHERE  AES_ENCRYPT(@password,@username) = `password` and prem_id is not null", cred, transaction: tran).ToList();

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
                                        message.Subject = "Premiere OTP";

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
        public ResponseModel InserNewDoctor(DoctorModel doctors_info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try {
                      
                        string query = $@"SELECT * FROM prem_doctor_usermaster WHERE username=@username";
                        var data = con.Query<string>(query, doctors_info, transaction: tran);   
                        string getcellno = $@"SELECT cellno FROM empmast emp JOIN docmaster doc ON CONCAT(emp.lastname,' ,',emp.firstname,' ',emp.middlename)=CONCAT(doc.lastname,' ,',doc.firstname,' ',doc.middlename) WHERE idno=@doccode";
                        var querycellno = con.QuerySingleOrDefault<string>(getcellno ,doctors_info, transaction: tran);

                        if (data.Count() == 0)
                        {
                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                            int insert_user_information = con.Execute($@"INSERT INTO prem_doctor_usermaster SET doccode=@doccode,fullname=CONCAT(@lastname,' ,',@firstname,' ',@middlename),username=@username,password=AES_ENCRYPT(@password,@username),createdAt=NOW()",
                                                                    doctors_info, transaction: tran);

                            if (insert_user_information >= 0)
                            {
                                    int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  set otp={otpnumber},toUserName=@username,date_expire=NOW(),createdAt=NOW()",
                                                                     doctors_info, transaction: tran);

                                if (insert_user_otp >= 0)
                                {
                                    int insert_message_otp = con.Execute($@"INSERT INTO messageout SET MessageTo='{querycellno}',MessageText='This is your OTP {otpnumber} it will expire in 300 seconds'",
                                                                  doctors_info, transaction: tran);
                                    if (insert_message_otp >= 0)
                                    {
                                        //var message = new MimeMessage();
                                        //string hospitalame = company.CompanyName().data.ToString();
                                        //message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
                                        //message.To.Add(new MailboxAddress(data, emailladdress));
                                        //message.Subject = "Doctors Portal App OTP";

                                        //message.Body = new TextPart("plain")
                                        //{
                                        //    Text = "This is your OTP " + otpnumber + " it will expire in 300 seconds"
                                        //};
                                        //using (var client = new SmtpClient())
                                        //{
                                        //    client.Connect("smtp.gmail.com", 587, false);


                                        //    client.Authenticate(DefaultConfig._providerEmailAddress, DefaultConfig._providerEmailPass);

                                        //    client.Send(message);
                                        //    client.Disconnect(true);
                                        //}
                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            success = true,
                                            message = "The User credentials has been Added sucessfully.",
                                            other_info = otpnumber

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
                                        message = "Error! Insert usermaster Failed."
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
                        string emailname = $@"SELECT fullname FROM prem_usermaster WHERE username=@username and activated='N'";
                        var dataemailname = con.Query<string>(query, user_info, transaction: tran);
                        string emailladdress = $@"SELECT email FROM prem_usermaster WHERE username=@username and activated='N'";
                        var dataemailladdress = con.Query<string>(query, user_info, transaction: tran);

                        if (data.Count() != 0)
                        {
                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                                int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  (id,otp,toUserName,date_expire,createdAt) VALUES(NULL,{otpnumber},@username,NOW(),NOW())",
                                                       user_info, transaction: tran);
                                if (insert_user_otp >= 0)
                                {
                                var message = new MimeMessage();
                                string hospitalame = company.CompanyName().data.ToString();
                                message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
                                message.To.Add(new MailboxAddress(emailname, emailladdress));
                                message.Subject = "Premiere OTP";

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
                                        message = "Successfully resend new OTP",
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
        public ResponseModel OTP_for_UpdatePassword(PAddNewOTP user_info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string query = $@"SELECT * FROM prem_usermaster WHERE username=@username";
                        var data = con.Query<string>(query, user_info, transaction: tran);

                        string emailname = $@"SELECT fullname FROM prem_usermaster WHERE username=@username";
                        string dataemailname = con.QuerySingleOrDefault<string>(emailname, user_info, transaction: tran);

                        string emailladdress = $@"SELECT TRIM(email) email FROM prem_usermaster WHERE username=@username";
                        string dataemailladdress = con.QuerySingleOrDefault<string>(emailladdress, user_info, transaction: tran);

                        if (data.Count() != 0)
                        {
                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                                int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  (id,otp,toUserName,date_expire,createdAt) VALUES(NULL,{otpnumber},@username,NOW(),NOW())",
                                                       user_info, transaction: tran);
                                if (insert_user_otp >= 0)
                                {
                                var message = new MimeMessage();
                                string hospitalame = company.CompanyName().data.ToString();
                                message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
                                message.To.Add(new MailboxAddress(dataemailname, dataemailladdress.Trim()));
                                message.Subject = "Premiere OTP";

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
                                        message = "Successfully resend new OTP",
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
                  
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Wrong Usename/Password. Please Try again"
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
        public ResponseModel update_password(mdlupdatepassword update_password)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                    
                            
                                int insert_user_otp = con.Execute($@"UPDATE prem_usermaster SET `password`=AES_ENCRYPT(@password,@username) WHERE username=@username",
                                                       update_password, transaction: tran);
                                if (insert_user_otp >= 0)
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Password updated successfully"
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
        public ResponseModel deletelastOTP(mdlLocked username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                    
                            
                                int insert_user_otp = con.Execute($@"DELETE FROM prem_otp WHERE toUsername=@username",
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
        public ResponseModel updateimg(setNew_profile prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        foreach (var f in prem.profile_image)
                        {
                            FileResponseModel file_upload_response = new FileResponseModel
                            {
                                success = true
                            };
                            var proc_file_payload = new Entities.UserEntity.UserImageFile();
                            file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources\Images\{prem?.username}\", f.FileName);
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
                                proc_file_payload.profile_image = file_upload_response.data.path;
                                proc_file_payload.profile_image_name = file_upload_response.data.name;
                            }
                            int insert_user_profile = con.Execute($@"UPDATE prem_usersinfo SET img='Resources\\Images\\{prem?.username}\\{file_upload_response.data.name}' WHERE username=@username",
                                             prem, transaction: tran);
                            if (insert_user_profile <= 0)
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Something went wrong updating profile picture"
                                };

                            }
                     

                        }
                 
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Profile picture updated successfully"
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
        public ResponseModel updatedocs(setNew_Docs prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var f in prem.Document_file)
                        {
                            FileResponseModel file_upload_response = new FileResponseModel
                            {
                                success = true
                            };
                            var proc_file_payload = new Entities.UserEntity.UserDocsFile();
                            file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources\Images\{prem?.username}\Documents\", f.FileName);
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
                                proc_file_payload.profile_image = file_upload_response.data.path;
                                proc_file_payload.profile_image_name = file_upload_response.data.name;
                            }

                            int insert_user_file = con.Execute($@"UPDATE prem_usersinfo SET docs='Resources\\Images\\{prem?.username}\\Documents\\{file_upload_response.data.name}' WHERE username=@username",
                                                       prem, transaction: tran);
                            if (insert_user_file <= 0)
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Document Not Updated"
                                };

                            }
                           
                        }
                        tran.Commit();
                        return new ResponseModel
                        {
                            success = true,
                            message = "Document Updated Successfully"
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
        public ResponseModel getUserInfoAdmin(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.QuerySingle($@"SELECT pum.image_path,pd.`deptname`,u.empname,emp.* FROM empmast emp JOIN usermaster u ON emp.idno=u.username LEFT JOIN prem_dept pd ON pd.`deptcode`=emp.deptcode LEFT JOIN prem_user_images pum ON pum.user_id=emp.empno WHERE u.username= @username",username, transaction: tran);
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
        public ResponseModel getDoctorInfo(Doctors_info.get_info info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.QuerySingle($@"SELECT u.prem_id,pum.image_path,pd.`deptname`,u.fullname,emp.* FROM empmast emp LEFT JOIN prem_doctor_usermaster u ON emp.idno=u.doccode LEFT JOIN prem_dept pd ON pd.`deptcode`=emp.deptcode LEFT JOIN prem_user_images pum ON pum.user_id=emp.empno WHERE u.username= @username", info, transaction: tran);
                        string brand_logo = defaults.hospitalLogo().data.ToString();
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(info.username, QRCodeGenerator.ECCLevel.Q);
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
        public ResponseModel getDoctorsExist(GetDoctorIdno idno)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.QuerySingle($@"SELECT emp.* FROM empmast emp JOIN docmaster doc ON CONCAT(emp.lastname,' ,',emp.firstname,' ',emp.middlename)=CONCAT(doc.lastname,' ,',doc.firstname,' ',doc.middlename)  WHERE idno=@idno",
                            idno, transaction: tran
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
        public ResponseModel getDoctorsUsernameExist(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.QuerySingle($@"select username from prem_doctor_usermaster where username=@username",
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
        public ResponseModel getcurrentotp(getCurrentOtp currentOtp)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {

                    string brand_logo = defaults.hospitalLogo().data.ToString();
                    var data = con.QuerySingleOrDefault<string>($@"SELECT otp FROM prem_otp WHERE toUserName=@username  and otp=@otp and TIMESTAMPDIFF(SECOND,date_expire,NOW())<=300 Limit 1",
                         currentOtp, transaction: tran);
                    if (data != null)
                    {
                        return new ResponseModel
                        {
                            success = true,
                            message = "OTP Granted",
                        };
                    }
                    else
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = "Incorrect OTP"
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
                        var data = con.QuerySingle($@"select prem_id from prem_usermaster where username=@username",
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
        public ResponseModel UpdateUserImage(UserModel.setUserImage userImage)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string query = $@"SELECT user_id FROM prem_user_images WHERE user_id=@user_id";
                        var data = con.Query<string>(query, userImage, transaction: tran);
                        if (data.Count() == 0)
                        {
                            foreach (var f in userImage.user_image)
                            {

                                FileResponseModel file_upload_response = new FileResponseModel
                                {
                                    success = true
                                };

                                var proc_file_payload = new Entities.UserEntity.UserImageFile()
                                {
                                    user_id = userImage.user_id
                                };

                                file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources\UserImages\{userImage.user_id}\", userImage.user_id.ToString()); ;
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
                                    proc_file_payload.file_dest = file_upload_response.data.path;
                                    proc_file_payload.file_name = file_upload_response.data.name;
                                }


                                int add_file = con.Execute(@"
                                INSERT INTO `prem_user_images` 
                                SET 
                                user_id=@user_id, 
                                image_path=@file_dest,
                                image_name=@file_name,
                                createdDate=NOW();",
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
                        else
                        {
                            foreach (var f in userImage.user_image)
                            {

                                FileResponseModel file_upload_response = new FileResponseModel
                                {
                                    success = true
                                };

                                var proc_file_payload = new Entities.UserEntity.UserImageFile()
                                {
                                    user_id = userImage.user_id
                                };

                                file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources\UserImages\{userImage.user_id}\", userImage.user_id.ToString()); ;
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
                                    proc_file_payload.file_dest = file_upload_response.data.path;
                                    proc_file_payload.file_name = file_upload_response.data.name;
                                }


                                int add_file = con.Execute(@"
                                update `prem_user_images` 
                                SET 
                                image_path=@file_dest,
                                image_name=@file_name where user_id=@user_id",
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
                            message = "Profile picture updated sucessfully."
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
        public ResponseModel getDoctorUserOTP(PGetUsername username)
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
                              
                                    string userappid = $@"SELECT NextDoctorAPPID()";
                                    string getuserappid = con.QuerySingleOrDefault<string>(userappid, username, transaction: tran);
                                    if (getuserappid != null)
                                    {
                                        int updateumappid = con.Execute($@"UPDATE prem_doctor_usermaster SET prem_id='{getuserappid}' WHERE username=@username",
                                                       username, transaction: tran);

                                        if (updateumappid == 1)
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
                                            message = "User not exist"
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
