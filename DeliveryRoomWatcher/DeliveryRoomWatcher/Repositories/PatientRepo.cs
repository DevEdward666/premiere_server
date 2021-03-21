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
    public class PatientRepo
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        public ResponseModel getUserstable()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pu.prem_id,pu.firstname,pu.middlename,pu.lastname FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username=pum.username",
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
        public ResponseModel getUserstableForApproval()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pu.prem_id,pu.firstname,pu.middlename,pu.lastname FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username=pum.username where pum.active='false'",
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
        public ResponseModel getUserInfo(PUsers.PGetUsersInfo prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pu.img,pu.docs,pu.prem_id,pu.email,pu.mobileno,pu.birthdate,pum.fullname, pum.active,r.regiondesc,cm.citymundesc,p.provincedesc,b.barangaydesc,CASE WHEN pu.gender='M' THEN 'Male' ELSE 'Female' END gender  FROM prem_usersinfo pu 
                                                JOIN prem_usermaster pum ON pu.username=pum.username 
                                                JOIN region r 
                                                ON pu.region_code=r.regioncode
                                                JOIN citymunicipality cm 
                                                ON pu.city_code=cm.citymuncode
                                                JOIN province p
                                                ON pu.province_code=p.provincecode
                                                JOIN barangay b 
                                                ON pu.barangay_code=b.barangaycode  WHERE pu.prem_id=@premid",
                          prem, transaction: tran
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
       
        public ResponseModel UpdateUser(PUsers.PUpdateUserInfo prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                     

                            int insert_user_otp = con.Execute($@" UPDATE prem_usermaster SET active='true',ApproveAt=NOW() WHERE prem_id=@premid",
                                                   prem, transaction: tran);
                            if (insert_user_otp >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The User Approved sucessfully."
                                };

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Approve user Failed."
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
        public ResponseModel addDiagnosticAppointment(PUsers.PAddDiagnosticAppointment prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string AppointmentID = $@"SELECT NextAppointReqId()";
                        string getAppointmentID = con.QuerySingleOrDefault<string>(AppointmentID, prem, transaction: tran);
                        if (getAppointmentID != null)
                        {
                            int insert_appointment = con.Execute($@"INSERT INTO ddt_appointment (appointmentno,prem_id,firstname,middlename,lastname,suffix,gender,csid,natid,relid,birthdate,email,mobno,line1,line2,brgycode,provincecode,citymuncode,regioncode,zipcode,reqreason,requestedat) 
                                                (SELECT '{getAppointmentID}',@premid, pu.firstname,pu.middlename,pu.lastname,pu.suffix,pu.gender,pu.civil_status,pu.nationality_code,pu.religion_code, pu.birthdate,pu.email,pu.mobileno,pu.address,pu.address,pu.barangay_code,pu.province_code,pu.city_code,pu.region_code,pu.zipcode,@reason,NOW() FROM prem_usersinfo pu 
                                                JOIN prem_usermaster pum ON pu.username = pum.username
                                                 WHERE pu.prem_id =@premid)",
                                               prem, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                              
                                    int insert_appointment_log = con.Execute($@"INSERT INTO ddt_appointmentlog (appointmentno,logevent,stsid,encodedat,encodedby) VALUES('{getAppointmentID}','request','1',NOW(),@premid)",
                                              prem, transaction: tran);

                                    if (insert_appointment_log >= 0)
                                    {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "The Diagnostic Appointment Added sucessfully."
                                    };


                                }
                                    else {
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = "Error! Insertion of Log Failed."
                                        };
                                    }
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Insertion of Appointment Failed."
                                };
                            }



                        }
                        else {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! Appointment ID NULL."
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

        } public ResponseModel addDiagnosticAppointmentothers(PUsers.PAddDiagnosticAppointmentOthers prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string AppointmentID = $@"SELECT NextAppointReqId()";
                        string getAppointmentID = con.QuerySingleOrDefault<string>(AppointmentID, prem, transaction: tran);
                        if (getAppointmentID != null)
                        {
                            int insert_appointment = con.Execute($@"INSERT INTO ddt_appointment (appointmentno,prem_id,prefix,firstname,middlename,lastname,suffix,gender,csid,natid,relid,birthdate,email,mobno,line1,line2,brgycode,provincecode,citymuncode,regioncode,zipcode,reqreason,requestedat)values
                                                ('{getAppointmentID}',@premid,@prefix,@firstname,@middlename,@lastname,@suffix,@gender,@civil_status,@nationality_code,@religion_code, @birthdate,@email,@mobile,
                                                @fulladdress,@fulladdress2,@barangay,@province_code,@city_code,@region_code,@zipcode,@reason,NOW())",
                                               prem, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                              
                                    int insert_appointment_log = con.Execute($@"INSERT INTO ddt_appointmentlog (appointmentno,logevent,stsid,encodedat,encodedby) VALUES('{getAppointmentID}','request','1',NOW(),@premid)",
                                              prem, transaction: tran);

                                    if (insert_appointment_log >= 0)
                                    {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "The Diagnostic Appointment Added sucessfully."
                                    };


                                }
                                    else {
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = "Error! Insertion of Log Failed."
                                        };
                                    }
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Insertion of Appointment Failed."
                                };
                            }



                        }
                        else {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! Appointment ID NULL."
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
        public ResponseModel addDiagnosticProcedure(PUsers.PAddDiagnosticAppointmentProcedure prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string lastAppointmentID = $@"SELECT MAX( appointmentno ) FROM ddt_appointment;";
                        string getlastAppointmentID = con.QuerySingleOrDefault<string>(lastAppointmentID, prem, transaction: tran);
                        if (lastAppointmentID != null)
                        {
                            int insert_appointment_procedure = con.Execute($@"INSERT INTO ddt_proc (appointmentno,proccode,availprice) VALUES('{getlastAppointmentID}',@proccode, (SELECT regprice FROM `ddt_prochdr` WHERE proccode  = @proccode LIMIT 1))",
                                               prem, transaction: tran);
                            if (insert_appointment_procedure >= 0)
                            {

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The Diagnostic Appointment Added sucessfully."
                                };

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Insertion of Procedure Failed."
                                };
                            }
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! Insertion of Procedure Failed."
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
        public ResponseModel DeactivateUser(PUsers.PUpdateUserInfo prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        int insert_user_otp = con.Execute($@" UPDATE prem_usermaster SET active='true',ApproveAt=NOW() WHERE prem_id=@premid",
                                               prem, transaction: tran);
                        if (insert_user_otp >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "The User Deactivated sucessfully."
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! Deactivation user Failed."
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
