using Dapper;
using ExecutiveAPI.Config;
using ExecutiveAPI.Model;
using ExecutiveAPI.Model.Common;
using ExecutiveAPI.Model.PatientsModel;
using MailKit.Net.Smtp;
using MimeKit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Repositories
{
    public class UserRepo
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        public List<UserModel.Users> authenticateUser(UserModel.AuthUser cred)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    var user_credentials = con.Query<UserModel.Users>($@"SELECT `username` FROM `prem_usermaster` WHERE  AES_ENCRYPT(@password,@username) = `password` and activated=true", cred, transaction: tran).ToList();

                    return user_credentials;
                }
            }
        }
        public ResponseModel OTP_For_Laboratory(OTPModel otpModel)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string query = $@"SELECT * FROM ddt_lab_req_mast WHERE req_pk=@req_pk";
                        var data = con.Query<string>(query, otpModel, transaction: tran);

                        string emailname = $@"SELECT CONCAT(last_name,' ',first_name,' ',middle_name) fullname FROM ddt_lab_req_mast WHERE req_pk=@req_pk";
                        string dataemailname = con.QuerySingleOrDefault<string>(emailname, otpModel, transaction: tran);

                        string emailladdress = $@"SELECT TRIM(email) email FROM ddt_lab_req_mast WHERE req_pk=@req_pk";
                        string dataemailladdress = con.QuerySingleOrDefault<string>(emailladdress, otpModel, transaction: tran);
                        string mobileno = $@"SELECT TRIM(mob_no) mobileno FROM ddt_lab_req_mast WHERE req_pk=@req_pk";
                        string datamobileno = con.QuerySingleOrDefault<string>(mobileno, otpModel, transaction: tran);

                        if (data.Count() != 0)
                        {
                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                            con.Execute($@"DELETE from prem_otp where toUserName=@req_pk;"
                                       , otpModel, transaction: tran);

                            int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  (id,otp,toUserName,date_expire,createdAt) VALUES(NULL,{otpnumber},@req_pk,NOW(),NOW())",
                                                   otpModel, transaction: tran);
                            if (insert_user_otp >= 0)
                            {
                                var message = new MimeMessage();
                                string hospitalame = "TUO IT Solutions";
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
                                if (insert_user_otp >= 0)
                                {
                                    int insert_message_otp = con.Execute($@"INSERT INTO messageout SET MessageTo='{datamobileno}',MessageText='This is your OTP {otpnumber} it will expire in 300 seconds'",
                                                                  otpModel, transaction: tran);
                                    if (insert_message_otp >= 0)
                                    {
                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            success = true,
                                            message = "Successfully send OTP",
                                            other_info = otpnumber
                                        };
                                    }
                                    else
                                    {
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = "QR not recognize. Please Try again"
                                        };
                                    }
                                }
                                else
                                {
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = "QR not recognize. Please Try again"
                                    };
                                }



                            }
                            else
                            {

                                return new ResponseModel
                                {
                                    success = false,
                                    message = "QR not recognize. Please Try again"
                                };

                            }
                        }
                        else
                        {

                            return new ResponseModel
                            {
                                success = false,
                                message = "QR not recognize. Please Try again"
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
        public ResponseModel get_patient_lab_data(OTPModel otpModel)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string OTP_code = $@"SELECT otp FROM prem_otp WHERE toUserName=@req_pk AND otp=@otp";
                        string otp_data = con.QuerySingleOrDefault<string>(OTP_code, otpModel, transaction: tran);
                        if (otp_data!=null)
                        {
                            var query = $@"SELECT req_pk,CONCAT(last_name,',',first_name,' ',middle_name) AS fullname,birth_date,requested_at,result_at FROM ddt_lab_req_mast WHERE req_pk=@req_pk";
                            var data = con.Query(query, otpModel, transaction: tran);


                            if (data.Count() != 0)
                            {
                                return new ResponseModel
                                {
                                    success = true,
                                    data = data
                                };
                            }
                            else
                            {

                                return new ResponseModel
                                {
                                    success = false,
                                    message = "QR not recognize. Please Try again"
                                };

                            }
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "OTP not recognize. Please Try again"
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
