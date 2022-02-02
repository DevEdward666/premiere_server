using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class DiagnosticRepository
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        public ResponseModel getAppointmentsRequestTable(PDIagnostics prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT DISTINCT dlrr.`req_pk` as appointmentno,CONCAT(dlrr.`first_name`,' ',dlrr.`middle_name`,' ',dlrr.`last_name`) AS fullname,ds.sts_desc AS STATUS,requested_at AS encodedat FROM ddt_lab_req_mast dlrr JOIN ddt_lab_req_proc dlrp ON dlrr.`req_pk` = dlrp.`req_pk` JOIN ddt_status ds ON dlrr.sts_pk=ds.sts_pk WHERE prem_id = @premid  AND ds.`sts_desc`='for approval' LIMIT @offset",
                                            prem,transaction: tran);

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
        public ResponseModel getLabReqPdf(string file_url)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {

                        return new ResponseModel
                        {
                            success = true,
                            data = Convert.ToBase64String(UseFtp.DownloadFtp(file_url, DefaultConfig.ftp_user, DefaultConfig.ftp_pass))
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
        public ResponseModel getAppointmentsRequestTableFinished(PDIagnostics prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT DISTINCT ap.req_pk as appointmentno,CONCAT(ap.first_name,' ',ap.middle_name,' ',ap.last_name) AS fullname,ds.`sts_desc` AS STATUS,ap.`requested_at` AS encodedat FROM ddt_lab_req_mast ap JOIN ddt_lab_req_proc pr ON ap.req_pk = pr.req_pk JOIN ddt_status ds ON ds.sts_pk=ap.sts_pk WHERE prem_id = @premid AND ap.sts_pk = 'f' LIMIT @offset",
                                            prem,transaction: tran);

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

        } public ResponseModel getAppointmentsResultsList(PDIagnostics prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  ds.sts_desc AS description,da.req_pk as appointmentno,dp.path AS resurl,DATE_FORMAT(da.result_at,'%Y-%m-%d %h:%m %p') finishedat 
                        FROM ddt_lab_req_mast da JOIN ddt_lab_req_result dp ON da.`req_pk` = dp.`req_pk` JOIN ddt_status ds ON ds.`sts_pk`=da.`sts_pk` WHERE da.`prem_id` = @premid  LIMIT @offset ",
                                            prem,transaction: tran);

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
    }
}
