using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
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
                        var data = con.Query($@"SELECT pu.img,pu.prem_id,pu.firstname,pu.middlename,pu.lastname FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username=pum.username where pum.active='false'",
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

        }   public ResponseModel getUserstableForMedicalRecordsLink()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  p.prem_id,pl.patno,p.img,p.firstname,p.middlename,p.lastname,p.birthdate,p.email FROM prem_linkrequest pl JOIN prem_usersinfo p ON pl.prem_id=p.prem_id WHERE pl.status='pending' ",
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
                        var data = con.Query($@"SELECT DISTINCT pu.patno, pu.img,pu.docs,pu.prem_id,pu.email,pu.mobileno,pu.birthdate,pum.fullname, pum.active,r.regiondesc,cm.citymundesc,p.provincedesc,b.barangaydesc,CASE WHEN pu.gender='M' THEN 'Male' ELSE 'Female' END gender  FROM prem_usersinfo pu 
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
       public ResponseModel getmedicalrecordsList(PUsers.PGetSingleMedicalRecords prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT inp.`patno`,inp.`hospitalno` ,CONCAT(ptm.`lastname`,', ',`ptm`.`firstname`,' ',COALESCE(ptm.`middlename`,'')) AS patientname,
                                          ptm.`firstname`,ptm.middlename,ptm.`lastname`,COALESCE(IF((ptm.`sex` = 'M'),'Male','Female'),'') AS gender,
                                          COALESCE(CivilStatus(`ptm`.`cs`),'') AS civilstatus,inp.`nationality`,inp.`religion`,ptm.`birthdate`,
                                          ((YEAR(inp.`admissiondate`) - YEAR(ptm.`birthdate`)) - (SUBSTR(inp.`admissiondate`,6,5) < RIGHT(ptm.`birthdate`,5))) AS Age,
                                          inp.`admissiondate`,
                                          (SELECT
                                             CONCAT(rt.`roomcode`,'|',`rt`.`bedno`,'|',`rt`.`nsunit`) AS patref
                                           FROM roomtran rt
                                           WHERE (rt.`patno` = inp.`patno`)
                                                  AND (rt.`roomtype` <> 'P')
                                                  AND ISNULL(rt.`statustag`)
                                           ORDER BY rt.`roomin` DESC
                                           LIMIT 1) AS roombedns,
                                          (SELECT
                                             dm.doccode AS docname
                                           FROM doctortran dt
                                              LEFT JOIN docmaster dm
                                                ON dm.`doccode` = dt.`doccode`
                                           WHERE dt.`patno` = inp.`patno`
                                                  AND dt.`admdoctag` = 'F'
                                           ORDER BY dt.`servicebegin`
                                           LIMIT 1) AS doccode,
                                          cl.`ChiefComplaint`   AS complaint,
                                          inp.`admdiagnosis`    AS admdiagnosis,
                                          inp.`medtype`         AS medtype
                                        FROM inpmaster inp
                                              LEFT JOIN clinicalsummary cl
                                                ON cl.`PatNo` = inp.`patno`
                                             LEFT JOIN patmaster ptm
                                               ON ptm.`hospitalno` = inp.`hospitalno`
                                        WHERE
                                        inp.`dischargedate` IS NULL
                                        AND inp.`cancelled` = 'N' AND inp.`hospitalno`=@hospitalno
                                        ORDER BY CONCAT(ptm.`lastname`,',',`ptm`.`firstname`,' ',`ptm`.`middlename`),`inp`.`admissiondate`",
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
        public ResponseModel UpdateMedicalRecordsLink(PUsers.PUpdateLinkMedicalRecords prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        if (!prem.status.Equals("declined"))
                        {
                            string check_exist = $@"SELECT patno from prem_usersinfo where patno IS NOT NULL and prem_id=@premid";
                            string check_if_exist = con.QuerySingleOrDefault<string>(check_exist, prem, transaction: tran);
                            if (check_if_exist == null)
                            {


                                int update_user_medicallink = con.Execute($@"UPDATE prem_linkrequest SET status=@status WHERE prem_id=@premid and patno=@patno",
                                                       prem, transaction: tran);
                                if (update_user_medicallink >= 0 && !prem.status.Equals("declined"))
                                {
                                    int update_user_prem_usersinfo = con.Execute($@"UPDATE prem_usersinfo SET patno=@patno WHERE prem_id=@premid ",
                                                           prem, transaction: tran);
                                    if (update_user_prem_usersinfo >= 0)
                                    {
                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            success = true,
                                            message = "The Medical Records Linked sucessfully."
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
                                else if (update_user_medicallink >= 0 && prem.status.Equals("declined"))
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "The Medical Records Linked sucessfully."
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
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Already Linked"
                                };
                            }
                        }
                        else {
                            int update_user_medicallink = con.Execute($@"UPDATE prem_linkrequest SET status=@status WHERE prem_id=@premid and patno=@patno",
                                                   prem, transaction: tran);
                            if (update_user_medicallink >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The Medical Records Linked sucessfully."
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
        public ResponseModel InsertLinkRequest(mdlLinkReq.request req)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string check_exist = $@"SELECT * from prem_linkrequest where patno=@patno and prem_id=@prem_id";
                        string check_if_exist = con.QuerySingleOrDefault<string>(check_exist, req, transaction: tran);
                        if (check_if_exist == null)
                        {

                            int insert_user_request_link = con.Execute($@" insert into prem_linkrequest (patno,prem_id,status) values(@patno,@prem_id,@status)",
                                                   req, transaction: tran);
                            if (insert_user_request_link >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The User Link Request sucessfully."
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
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "You already request linking for this account"
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
        public ResponseModel SyncFile(mdlPatientFiles.insertfile file)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                      

                            int insert_user_request_link = con.Execute($@"insert into prem_patient_files (prem_id,filename,type,deviceStored,createdAt) values(@prem_id,@filename,@type,@deviceStored,NOW())",
                                                   file,transaction: tran);
                            if (insert_user_request_link >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The User file inserted sucessfully."
                                };

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! file insertion Failed."
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
        public ResponseModel getpatientfiles(mdlPatientFiles.getfile files)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_patient_files WHERE prem_id=@prem_id AND type=@type and deviceStored=@deviceStored",
                                            files, transaction: tran);

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
        public ResponseModel addDiagnosticAppointment(PUsers.PAddDiagnosticAppointment prem)
        {
            ResponseModel response = new ResponseModel();
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
                            int insert_appointment = con.Execute($@"INSERT INTO ddt_lab_req_mast (req_pk,prem_id,reason,first_name,middle_name,last_name,suffix,gender, cs_pk,nat_pk,nat_desc,rel_pk,rel_desc,birth_date,email,mob_no,local_address,brgy_pk,prov_pk,city_pk,region_pk,psgc_address, zip_code,requested_at,tran_method,req_total_cost,sts_pk) 
                            (SELECT '{getAppointmentID}',@premid,@reason,pu.firstname,pu.middlename,pu.lastname,pu.suffix,pu.gender,pu.civil_status,pu.nationality_code,pu.nationality_code,pu.religion_code,pu.religion_code,pu.birthdate,pu.email,pu.mobileno,pu.address,pu.barangay_code,pu.province_code,pu.city_code,pu.region_code,CONCAT(db.barangaydesc,',',city.citymundesc,',',dp.provincedesc),pu.zipcode,NOW(),'o',@req_total_cost,'p'
                            FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username = pum.username JOIN ddt_province dp ON dp.provincecode = pu.province_code JOIN ddt_barangay db ON db.barangaycode = pu.barangay_code JOIN citymunicipality city ON city.citymuncode = pu.city_code WHERE pu.prem_id = @premid)",
                                               prem, transaction: tran);
                            if (insert_appointment >= 0)
                            {

                                //int insert_appointment_log = con.Execute($@"INSERT INTO ddt_appointmentlog (appointmentno,logevent,stsid,encodedat,encodedby) VALUES('{getAppointmentID}','request','1',NOW(),@premid)",
                                //          prem, transaction: tran);

                                //if (insert_appointment_log >= 0)
                                //{
                                    foreach (var proc in prem.listofprocedures)
                                    {
                                        string sql_add_proc = $@"INSERT  INTO ddt_lab_req_proc SET req_pk='{getAppointmentID}',proccode=@proccode,procdesc=@procdesc,price=(SELECT regprice FROM `ddt_prochdr` WHERE proccode  = @proccode LIMIT 1),sts_pk='p'";

                                        int insert_appointment_procedure = con.Execute(sql_add_proc, proc, transaction: tran);

                                        if (insert_appointment_procedure >= 0)
                                        {

                                           
                                          
                                        }
                                    }
                                    tran.Commit();

                                    response.success = true;
                                    response.message = "The Diagnostic Appointment Added sucessfully.";

                                //}
                                //else
                                //{

                                //    response.success = false;
                                //    response.message = "Error! Insertion of Log Failed.";
                                //}
                            }
                            else
                            {
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
            return response;
        }
        public ResponseModel addDiagnosticAppointmentothers(PUsers.PAddDiagnosticAppointmentOthers Appointment)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string AppointmentID = $@"SELECT NextAppointReqId()";
                        string getAppointmentID = con.QuerySingleOrDefault<string>(AppointmentID, Appointment, transaction: tran);
                        if (getAppointmentID != null)
                        {
                            int insert_appointment = con.Execute($@"INSERT INTO ddt_lab_req_mast 
                            SET req_pk='{getAppointmentID}',prem_id=@premid,reason=@reason,prefix=@prefix,first_name=@firstname,middle_name=@middlename
                            ,last_name=@lastname,suffix=@suffix,gender=@gender,cs_pk=@civil_status_key,cs_desc=@civil_status_desc,nat_pk=@nationality_code,nat_desc=@nationality_code
                             ,birth_date=@birthdate,email=@email,mob_no=@mobile,local_address=@fulladdress,brgy_pk=@barangay,prov_pk=@province_code
                            ,city_pk=@city_code,region_pk=@region_code,psgc_address=@psgc_address,zip_code=@zipcode,tran_method='o',req_total_cost=@req_total,requested_at=NOW(),sts_pk='p'",
                                               Appointment, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                              
                                    //int insert_appointment_log = con.Execute($@"INSERT INTO ddt_appointmentlog (appointmentno,logevent,stsid,encodedat,encodedby) VALUES('{getAppointmentID}','request','1',NOW(),@premid)",
                                    //          Appointment, transaction: tran);

                                    //if (insert_appointment_log >= 0)
                                    //{
                                    foreach (var proc in Appointment.listofprocedures)
                                    {
                                        int i = Appointment.listofprocedures.Count;
                                        int x = 0;
                                        string sql_add_proc = $@"INSERT INTO ddt_lab_req_proc SET req_pk='{getAppointmentID}',proccode=@proccode,procdesc=@procdesc,price=(SELECT regprice FROM `ddt_prochdr` WHERE proccode  = @proccode LIMIT 1),sts_pk='p'";

                                        int insert_appointment_procedure = con.Execute(sql_add_proc, proc, transaction: tran);
                     
                                        if (insert_appointment_procedure < 0)
                                        {

                                            response.success = false;
                                            response.message = "Error! Insertion of Log Failed.";

                                        }
                                       
                                    }
                                    tran.Commit();

                                    response.success = true;
                                    response.message = "The Diagnostic Appointment Added sucessfully.";
                              

                                //} else {

                                //    response.success = false;
                                //    response.message = "Error! Insertion of Log Failed.";
                                        
                                //    }
                            }
                            else
                            {

                                response.success = false;
                                response.message = "Error! Insertion of Appointment Failed.";
                                
                            }

                        } else
                        {
                          
                            response.success = false;
                            response.message = "Error! Insertion of Appointment Failed.";
                            
                        }
                      

                    }
                    catch (Exception e)
                    {

                        response.success = false;
                        response.message = $@"External server error. {e.Message.ToString()}";
                        
                    }

                  
                }
            }
            return response;
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
