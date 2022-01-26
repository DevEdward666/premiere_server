using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Clinic;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using MimeKit;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class PatientRepo
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        CompanyRepository company = new CompanyRepository();
        public ResponseModel getConsultInfo(consult_info info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  sm.`sts_desc`,cr.consult_req_pk AS appointment_id,cr.prem_id,cr.first_name,cr.middle_name,cr.last_name,
                                                cr.gender, dc.csdesc, nat.nationality, cr.rel_pk, cr.birth_date, cr.email,
                                                cr.mob_no, CONCAT(line1,',',line2) AS line1, db.barangaydesc, dp.provincedesc, city.citymundesc, dg.regiondesc, CONCAT(
                                                 db.barangaydesc, ',', city.citymundesc, ',', dp.provincedesc) AS line2, cr.zip_code,cr.`request_at`,cr.`paymongo_src_id`,  CASE WHEN paymongo_paid_at IS NOT NULL THEN 'PAID' ELSE 'NOT PAID' END payment_status
                                                 FROM consult_request cr 
                                                LEFT JOIN ddt_province dp ON dp.provincecode = cr.prov_pk LEFT JOIN ddt_barangay db ON db.barangaycode = cr.brgy_pk LEFT JOIN citymunicipality city ON city.citymuncode = cr.citymun_pk 
                                               LEFT JOIN ddt_region dg  ON dg.regioncode = cr.region_pk LEFT JOIN ddt_civilstatus dc ON dc.cskey=cr.cs_pk LEFT JOIN nationality nat ON nat.nat_pk=cr.nat_pk
                                                  JOIN status_master sm ON sm.`sts_pk`=cr.sts_pk   
                                                WHERE cr.prem_id =@prem_id AND cr.consult_req_pk=@consult_req_pk",
                         info, transaction: tran
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
                        var data = con.Query($@"SELECT DISTINCT pu.patno,pu.username, pu.img,pu.docs,pu.prem_id,pu.email,pu.mobileno,pu.birthdate,pum.fullname, pum.active,r.regiondesc,cm.citymundesc,p.provincedesc,b.barangaydesc,CASE WHEN pu.gender='M' THEN 'Male' ELSE 'Female' END gender  FROM prem_usersinfo pu 
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
                        var data = con.Query($@"SELECT inp.`patno`,inp.`hospitalno` ,r.`regiondesc`,pr.`provincedesc`,cm.`citymundesc`,br.`barangaydesc` ,ptm.`patreligion`,ptm.`lastname`,`ptm`.`firstname`,ptm.`middlename`,CONCAT(ptm.`lastname`,', ',`ptm`.`firstname`,' ',COALESCE(ptm.`middlename`,'')) AS patientname,
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
                                               CONCAT(dm.firstname,', ',dm.middlename,', ',dm.lastname,',',dm.doctitle ) AS docname
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
   LEFT JOIN region r ON r.`regioncode`=ptm.`perregion`
                                               LEFT JOIN province pr ON pr.`provincecode`=ptm.`perprovince`
                                               LEFT JOIN citymunicipality cm ON cm.`citymuncode`=ptm.`percitymun`
                                               LEFT JOIN barangay br ON br.`barangaycode`=ptm.`perbarangay`
                                        WHERE
                                        inp.`dischargedate` IS NOT NULL
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
                            int insert_appointment = con.Execute($@"INSERT INTO ddt_lab_req_mast (req_pk,prem_id,reason,first_name,middle_name,last_name,suffix,gender, cs_pk,nat_pk,rel_pk,rel_desc,birth_date,email,mob_no,local_address,brgy_pk,prov_pk,city_pk,region_pk,psgc_address, zip_code,requested_at,tran_method,req_total_cost,sts_pk) 
                            (SELECT '{getAppointmentID}',@premid,@reason,pu.firstname,pu.middlename,pu.lastname,pu.suffix,pu.gender,pu.civil_status,pu.nationality_code,pu.religion_code,pu.religion_code,pu.birthdate,pu.email,pu.mobileno,pu.address,pu.barangay_code,pu.province_code,pu.city_code,pu.region_code,CONCAT(db.barangaydesc,',',city.citymundesc,',',dp.provincedesc),pu.zipcode,NOW(),'o',@req_total_cost,'fa'
                            FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username = pum.username JOIN ddt_province dp ON dp.provincecode = pu.province_code JOIN ddt_barangay db ON db.barangaycode = pu.barangay_code JOIN citymunicipality city ON city.citymuncode = pu.city_code WHERE pu.prem_id = @premid)",
                                               prem, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                                foreach (var f in prem.attach_req_files)
                                {

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new DiagnosticRequestFileEntity()
                                    {
                                        req_pk = getAppointmentID
                                    };

                                    file_upload_response = UseFtp.UploadFtp(f, DefaultConfig.ftp_ip + ":" + DefaultConfig.ftp_port + "/" + DefaultConfig.app_name + $@"/Uploads/DiagnosticFiles/", DefaultConfig.ftp_user, DefaultConfig.ftp_pass);
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
                                        proc_file_payload.file_path = file_upload_response.data.path;
                                        proc_file_payload.file_name = file_upload_response.data.name;
                                    }


                                    int add_file = con.Execute(@"
                               INSERT INTO `ddt_lab_req_proc_file` 
                                SET 
                                req_pk=@req_pk, 
                                file_path=@file_path,
                                file_name=@file_name,
                                encoded_at=NOW();",
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

                                foreach (var proc in prem.listofprocedures)
                                    {
                                        string sql_add_proc = $@"INSERT  INTO ddt_lab_req_proc SET req_pk='{getAppointmentID}',proccode=@proccode,procdesc=@procdesc,price=(SELECT regprice FROM `ddt_prochdr` WHERE proccode  = @proccode LIMIT 1),sts_pk='fa'";

                                        int insert_appointment_procedure = con.Execute(sql_add_proc, proc, transaction: tran);

                                        if (insert_appointment_procedure >= 0)
                                        {

                                           
                                          
                                        }
                                    }
                                    tran.Commit();

                                    response.success = true;
                                    response.message = "The Diagnostic Appointment Added sucessfully.";

                       
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
                            ,last_name=@lastname,suffix=@suffix,gender=@gender,cs_pk=@civil_status_key,cs_desc=@civil_status_desc,nat_pk=@nationality_code
                             ,birth_date=@birthdate,email=@email,mob_no=@mobile,local_address=@fulladdress,brgy_pk=@barangay,prov_pk=@province_code
                            ,city_pk=@city_code,region_pk=@region_code,psgc_address=@psgc_address,zip_code=@zipcode,tran_method='o',req_total_cost=@req_total,requested_at=NOW(),sts_pk='fa'",
                                               Appointment, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                                foreach (var f in Appointment.attach_req_files)
                                {

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new DiagnosticRequestFileEntity()
                                    {
                                        req_pk = getAppointmentID
                                    };

                                    file_upload_response = UseFtp.UploadFtp(f, DefaultConfig.ftp_ip + ":" + DefaultConfig.ftp_port + "/" + DefaultConfig.app_name + "/Uploads/ConsultationFiles/", DefaultConfig.ftp_user, DefaultConfig.ftp_pass);
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
                                        proc_file_payload.file_path =file_upload_response.data.path;
                                        proc_file_payload.file_name = file_upload_response.data.name;
                                    }


                                    int add_file = con.Execute(@"
                               INSERT INTO `ddt_lab_req_proc_file` 
                                SET 
                                req_pk=@req_pk, 
                                file_path=@file_path,
                                file_name=@file_name,
                                encoded_at=NOW();",
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

                                foreach (var proc in Appointment.listofprocedures)
                                    {
                                        int i = Appointment.listofprocedures.Count;
                                        int x = 0;
                                        string sql_add_proc = $@"INSERT INTO ddt_lab_req_proc SET req_pk='{getAppointmentID}',proccode=@proccode,procdesc=@procdesc,price=(SELECT regprice FROM `ddt_prochdr` WHERE proccode  = @proccode LIMIT 1),sts_pk='fa'";

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
        DefaultsRepo def_val_repo = new DefaultsRepo();
        PaymentLinkEmail pay_link_email = new PaymentLinkEmail();
        public ResponseModel addClinicAppointment(ClinicModel prem)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        ImagePathtoFormFile path = new ImagePathtoFormFile();
                        //string otp_result = con.QuerySingleOrDefault<string>($@"
                        //            SELECT IF(COUNT(otp_code) > 0, IF(TIMESTAMPDIFF(SECOND,encoded_at,NOW())> 300 ,'e','v'), 'x') result FROM otp WHERE otp_code = @otp_code and mob_no = @mobile;
                        //            ",
                        // prem, transaction: tran);
                        //if (otp_result.Equals("e"))
                        //{
                        //    tran.Rollback();
                        //    return new ResponseModel
                        //    {
                        //        success = false,
                        //        message = "The OTP that you entered has already expired!"
                        //    };
                        //}
                        //else if (otp_result.Equals("x"))
                        //{
                        //    tran.Rollback();
                        //    return new ResponseModel
                        //    {
                        //        success = false,
                        //        message = "The OTP that you entered is not correct, kindly enter the correct and valid one."
                        //    };
                        //}
                        //prem.ProfileImage = path.ConvertFIle(prem);
                        string getmobile = $@"SELECT pu.mobileno FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username = pum.username  WHERE pu.prem_id = @premid";
                        string QueryGetMobile = con.QuerySingleOrDefault<string>(getmobile, prem, transaction: tran);

                        string getemail = $@"SELECT pu.email FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username = pum.username  WHERE pu.prem_id = @premid";
                        string QueryGetEmail = con.QuerySingleOrDefault<string>(getemail, prem, transaction: tran);

                        string AppointmentID = $@"SELECT next_consult_consult_req_pk(NULL)";
                        string getAppointmentID = con.QuerySingleOrDefault<string>(AppointmentID, prem, transaction: tran);
                        if (getAppointmentID != null)
                        {

                            //if (payload?.attach_profile_pic != null)
                            //{

                            //    FileResponseModel pat_pic_file_res = new FileResponseModel
                            //    {
                            //        success = true
                            //    };

                            //    pat_pic_file_res = UseFtp.UploadToFtp(payload.attach_profile_pic, DefaultConfig.ftp_ip, $"/{DefaultConfig.app_name}/Uploads/UserPhotos/", DefaultConfig.ftp_user, DefaultConfig.ftp_pass);


                            //    if (!pat_pic_file_res.success)
                            //    {
                            //        return new ResponseModel
                            //        {
                            //            success = false,
                            //            message = pat_pic_file_res.message
                            //        };
                            //    }
                            //    else
                            //    {
                            //        payload.pic_dest = pat_pic_file_res.data.path;
                            //    }

                            //}
                            int insert_appointment = con.Execute($@"INSERT INTO consult_request (consult_req_pk,prem_id,chief_complaint,symptoms,notes,first_name,middle_name,last_name,suffix,gender, cs_pk,nat_pk,rel_pk,birth_date,email,mob_no,line1,brgy_pk,prov_pk,citymun_pk,region_pk,line2, zip_code,request_at,sts_pk,   assign_dept_pk) 
                            (SELECT '{getAppointmentID}',@premid,@complaint,@symptomps,@remarks,pu.firstname,pu.middlename,pu.lastname,pu.suffix,pu.gender,pu.civil_status,pu.nationality_code,pu.religion_code,pu.birthdate,pu.email,pu.mobileno,pu.address,pu.barangay_code,pu.province_code,pu.city_code,pu.region_code,CONCAT(db.barangaydesc,',',city.citymundesc,',',dp.provincedesc),pu.zipcode,NOW(),'fa',@assign_dept_pk
                            FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username = pum.username JOIN ddt_province dp ON dp.provincecode = pu.province_code JOIN ddt_barangay db ON db.barangaycode = pu.barangay_code JOIN citymunicipality city ON city.citymuncode = pu.city_code WHERE pu.prem_id = @premid)",prem, transaction: tran);
                            
                            string getdetails = $@"SELECT '{getAppointmentID}' as appointment_id,@premid as premid,@complaint as complaint,@symptomps as symtomps,@remarks as notes,pu.firstname,pu.middlename,pu.lastname,pu.suffix,pu.gender,pu.civil_status,pu.nationality_code,pu.religion_code,pu.birthdate,pu.email,pu.mobileno,pu.address as line1 ,pu.barangay_code,pu.province_code,city.citymundesc,dg.regiondesc,CONCAT(db.barangaydesc,',',city.citymundesc,',',dp.provincedesc) as line2,pu.zipcode,NOW() as requested_at,'fa' as status
                            FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username = pum.username JOIN ddt_province dp ON dp.provincecode = pu.province_code JOIN ddt_barangay db ON db.barangaycode = pu.barangay_code JOIN citymunicipality city ON city.citymuncode = pu.city_code join ddt_region dg on dg.regioncode=pu.region_code  WHERE pu.prem_id = @premid";
                        var setdetails = con.Query(getdetails, prem, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                                foreach (var f in prem.consultation_files)
                                {

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new ConsultRequestFileEntity()
                                    {
                                        consult_req_pk = getAppointmentID
                                    };

                                    file_upload_response = UseFtp.UploadFtp(f, DefaultConfig.ftp_ip+":"+DefaultConfig.ftp_port + "/" + DefaultConfig.app_name + "/Uploads/ConsultationFiles/", DefaultConfig.ftp_user, DefaultConfig.ftp_pass);
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
                                INSERT INTO `consult_request_file` 
                                SET 
                                consult_req_pk=@consult_req_pk, 
                                file_dest=@file_dest,
                                file_name=@file_name,
                                encoded_at=NOW();",
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


                                string otp_code = "111111";

                                string sms_body = $@"Greetings from {def_val_repo.GetHospitalName().data}, your Out-Patient Telemedicine ePayLink OTP is {otp_code}. This is only valid within 24 hours.";

                                int message_affected_rows = con.Execute($@"INSERT INTO messageout SET messageto={QueryGetMobile},messagetext=@sms_body;", new { sms_body }, transaction: tran);

                                if (message_affected_rows > 0)
                                {
                                    con.Execute($@"DELETE from otp where mob_no=@mobile;"
                                        , new { prem.mobile }, transaction: tran);

                                    OtpEntity otp_payload = new OtpEntity
                                    {
                                        otp_code = otp_code,
                                        mob_no = QueryGetMobile,
                                        consult_req_pk = getAppointmentID,
                                        user_pk = QueryGetEmail
                                    };

                                    int insert_otp_affected_rows = con.Execute(
                                                $@"INSERT INTO OTP set user_pk='{QueryGetEmail}', mob_no='{QueryGetMobile}', consult_req_pk='{getAppointmentID}', otp_code=@otp_code;"
                                                , otp_payload, transaction: tran);

                                    if (insert_otp_affected_rows > 0)
                                    {
                                        prem.hash_key = con.QuerySingle<string>($@"SELECT MD5('{getAppointmentID}')", null, transaction: tran);
                                        prem.mobile = $@"{QueryGetMobile}";
                                        prem.consult_req_pk = $@"{getAppointmentID}";
                                        prem.email = $@"{QueryGetEmail}";
                                        ResponseModel email_response = pay_link_email.CreatePaymentLinkEmail(prem);

                                        if (!email_response.success)
                                        {
                                            return email_response;
                                        }
                                    }
                                }


                                tran.Commit();
                                return new ResponseModel
                                {               
                                    data=setdetails,
                                    success = true,
                                    message = "Your consultation request has been created. We will keep in touch for further details."
                                };
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "No affected rows when trying to save the consultation request. "
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
        }
        public ResponseModel addClinicAppointmentOthers(Models.Clinic.ClinicModel Appointment)
        {
            ResponseModel response = new ResponseModel();
            ImagePathtoFormFile path = new ImagePathtoFormFile();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                    
                        string AppointmentID = $@"SELECT next_consult_consult_req_pk(NULL)";
                        string getAppointmentID = con.QuerySingleOrDefault<string>(AppointmentID, Appointment, transaction: tran);
                        Appointment.consult_req_pk = getAppointmentID;
                        Appointment.appointment_id = getAppointmentID;
                   
                        if (getAppointmentID != null)
                        {

                            //string otp_result = con.QuerySingleOrDefault<string>($@"
                            //        SELECT IF(COUNT(otp_code) > 0, IF(TIMESTAMPDIFF(SECOND,encoded_at,NOW())> 300 ,'e','v'), 'x') result FROM otp WHERE otp_code = @otp_code and mob_no = @mobile;
                            //        ",
                            // Appointment, transaction: tran);
                            //if (otp_result.Equals("e"))
                            //{
                            //    tran.Rollback();
                            //    return new ResponseModel
                            //    {
                            //        success = false,
                            //        message = "The OTP that you entered has already expired!"
                            //    };
                            //}
                            //else if (otp_result.Equals("x"))
                            //{
                            //    tran.Rollback();
                            //    return new ResponseModel
                            //    {
                            //        success = false,
                            //        message = "The OTP that you entered is not correct, kindly enter the correct and valid one."
                            //    };
                            //}

                            int insert_appointment = con.Execute($@"INSERT INTO consult_request SET 
                             consult_req_pk=@consult_req_pk,
                             prem_id=@premid,
                             chief_complaint=@complaint,
                             symptoms=@symptomps,
                             prefix=@prefix,
                             first_name=@firstname,
                             middle_name=@middlename,
                             last_name=@lastname,
                             suffix=@suffix,
                             gender=@gender,
                             cs_pk=@civil_status_key,
                             nat_pk=@nationality_code,
                             birth_date=@birthdate,
                             email=@email,
                             mob_no=@mobile,
                             line1=@fulladdress,
                             line2=@psgc_address,
                             brgy_pk=@barangay,
                             prov_pk=@province_code,
                             citymun_pk=@city_code,
                             region_pk=@region_code,
                             zip_code=@zipcode,
                     assign_dept_pk=@assign_dept_pk,
                             request_at=NOW(),sts_pk='fa'",
                                               Appointment, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                                foreach (var f in Appointment.consultation_files)
                                {

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new ConsultRequestFileEntity()
                                    {
                                        consult_req_pk = getAppointmentID
                                    };

                                    file_upload_response = UseFtp.UploadFtp(f, DefaultConfig.ftp_ip + ":" + DefaultConfig.ftp_port + "/" + DefaultConfig.app_name + "/Uploads/ConsultationFiles/", DefaultConfig.ftp_user, DefaultConfig.ftp_pass);
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
                                INSERT INTO `consult_request_file` 
                                SET 
                                consult_req_pk=@consult_req_pk, 
                                file_dest=@file_dest,
                                file_name=@file_name,
                                encoded_at=NOW();",
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

                                string otp_code = "111111";

                                string sms_body = $@"Greetings from {def_val_repo.GetHospitalName().data}, your Out-Patient Telemedicine ePayLink OTP is {otp_code}. This is only valid within 24 hours.";

                                int message_affected_rows = con.Execute($@"INSERT INTO messageout SET
                                                                messageto='{Appointment.mobile}',
                                                                messagetext=@sms_body;"
                                                               , new { sms_body }, transaction: tran);

                                if (message_affected_rows > 0)
                                {
                                    con.Execute($@"DELETE from otp where mob_no=@mobile;"
                                        , new { Appointment.mobile }, transaction: tran);

                                    OtpEntity otp_payload = new OtpEntity
                                    {
                                        otp_code = otp_code,
                                        mob_no = Appointment.mobile,
                                        consult_req_pk = Appointment.consult_req_pk,
                                        user_pk = Appointment.email
                                    };

                                    int insert_otp_affected_rows = con.Execute(
                                                "INSERT INTO otp set user_pk=@user_pk, mob_no=@mob_no, consult_req_pk=@consult_req_pk, otp_code=@otp_code;"
                                                , otp_payload, transaction: tran);

                                    if (insert_otp_affected_rows > 0)
                                    {
                                        Appointment.hash_key = con.QuerySingle<string>($@"SELECT MD5('{Appointment.consult_req_pk}')", null, transaction: tran);

                                        ResponseModel email_response = pay_link_email.CreatePaymentLinkEmail(Appointment);

                                        if (!email_response.success)
                                        {
                                            return email_response;
                                        }
                                    }
                                }


                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Your consultation request has been created. We will keep in touch for further details."
                                };
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "No affected rows when trying to save the consultation request. "
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
        public ResponseModel updatepassbase(PUsers.UpdateUserInfo prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                      
                            int update_usermaster = con.Execute($@"UPDATE prem_usermaster SET active=@active,passbase_id=@passbase_id,passbase_status=@passbase_status WHERE username=@username",
                                           prem, transaction: tran);
                            if (update_usermaster >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Updated info"
                                };
                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Update user Failed."
                                };
                            }
                        
                      
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }
        public ResponseModel updateinfo(PUsers.UpdateUserInfo prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int update_user_info = con.Execute($@"UPDATE prem_usersinfo SET civil_status=@civil_status,region_code=@region_code,city_code=@city_code,province_code=@province_code,barangay_code=@barangay_code,religion_code=@religion_code,nationality_code=@nationality_code,address=@fulladdress WHERE username=@username",
                                               prem, transaction: tran);
                        if (update_user_info >= 0)
                        {
                            int update_user_master = con.Execute($@"UPDATE prem_usermaster SET passbase_status=@passbase_status,active='true',ApproveAt=NOW() WHERE username=@username",
                                     prem, transaction: tran);
                            if (update_user_master >= 0)
                            {
                                //FileResponseModel file_upload_response = new FileResponseModel
                                //{
                                //    success = true
                                //};
                                //FileResponseModel docsfile_upload_response = new FileResponseModel
                                //{
                                //    success = true
                                //};


                                //file_upload_response = UseLocalFiles.UploadLocalFile(prem.profilefile, $@"Resources\Images\{prem.username}\{prem.username}", prem.username);
                                //if (!file_upload_response.success)
                                //{
                                //    return new ResponseModel
                                //    {
                                //        success = false,
                                //        message = file_upload_response.message
                                //    };
                                //}
                                //docsfile_upload_response = UseLocalFiles.UploadLocalFile(prem.Docsfile, $@"Resources\Images\{prem.username}\{prem.username}_VerificationDocs", prem.username);
                                //if (!docsfile_upload_response.success)
                                //{
                                //    return new ResponseModel
                                //    {
                                //        success = false,
                                //        message = docsfile_upload_response.message
                                //    };
                                //}




                                //string path = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\\{prem.file.FileName}");
                                //if (!Directory.Exists(path))
                                //{
                                //    Directory.CreateDirectory(path);
                                //    string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\{prem.file.FolderName}\", prem.file.FileName);
                                //    using (Stream stream = new FileStream(filepath, FileMode.Create))
                                //    {
                                //        prem.file.FormFile.CopyTo(stream);
                                //    }

                                //    Directory.CreateDirectory(path);
                                //    string docsfilepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\{prem.docsfile.FolderName}\\", prem.docsfile.FileName);
                                //    using (Stream stream = new FileStream(docsfilepath, FileMode.Create))
                                //    {
                                //        prem.docsfile.FormFile.CopyTo(stream);
                                //    }




                                //}
                                //else
                                //{
                                //    string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\{prem.docsfile.FolderName}\", prem.docsfile.FileName);
                                //    using (Stream stream = new FileStream(filepath, FileMode.Create))
                                //    {
                                //        prem.file.FormFile.CopyTo(stream);
                                //    }

                                //    string docsfilepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\{prem.docsfile.FolderName}\", prem.docsfile.FileName);
                                //    using (Stream stream = new FileStream(docsfilepath, FileMode.Create))
                                //    {
                                //        prem.docsfile.FormFile.CopyTo(stream);
                                //    }

                                //}



                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The user info updated sucessfully."
                                };
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Update user Failed."
                                };
                            }

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! Update user Failed."
                            };
                        }
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }
        public ResponseModel InsertLinkOTP(Link_consultation_OTP username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string emailladdress = username.email;
                        string emailname = username.fullname;
                        string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                        string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                        int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  (id,otp,toUserName,date_expire,createdAt) VALUES(NULL,{otpnumber},@username,NOW(),NOW())",
                                               username, transaction: tran);
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
        public ResponseModel LinkOTPCosult(Link_consultation_OTP username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
              
                        string query = $@"SELECT otp FROM prem_otp WHERE toUserName=@username and TIMESTAMPDIFF(SECOND,date_expire,NOW())<=300 ORDER BY date_expire DESC  Limit 1";
                        string getUsernametoOTP = con.QuerySingleOrDefault<string>(query, username, transaction: tran);
                        if (getUsernametoOTP != null)
                        {
                            if (getUsernametoOTP.Equals( username.otp))
                            {
                                int updateuser = con.Execute($@"UPDATE consult_request SET prem_id=@prem_id WHERE consult_req_pk=@consult_req_pk",
                                                 username, transaction: tran);

                                if (updateuser == 1)
                                {


                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Consultation Link Complete. Thank You!"
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
                                    message = "OTP not exist."
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
        public ResponseModel Link_Consultation(Link_consultation link)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try { 
                    
                        string query = $@"SELECT * FROM consult_request WHERE consult_req_pk=@consult_req_pk AND email=@email AND prem_id IS NULL";
                        var data = con.Query<string>(query, link, transaction: tran);
                        if (data.Count() != 0)
                        {

                            string getotp = $@"SELECT LPAD(FLOOR(RAND() * 999999.99), 6, '0') as otpnumber";
                            string otpnumber = con.QuerySingleOrDefault<string>(getotp, null, transaction: tran);
                            int insert_user_information = con.Execute($@"INSERT INTO prem_link_consultation SET username=@username, fullname=@fullname, consult_req_pk=@consult_req_pk ,email=@email",
                                                                    link, transaction: tran);

                            if (insert_user_information >= 0)
                            {
                            
                        string emailladdress = link.email;
                        string emailname = link.fullname;
                                int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  set otp={otpnumber},toUserName=@username,date_expire=NOW(),createdAt=NOW()",
                                                                 link, transaction: tran);
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
                                        message = "OTP has been sent to your email registered to the consultation",
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
                                    message = "Error! Insert Failed."
                                };

                            }
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Consultation Not Exist"
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
