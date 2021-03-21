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
                        var data = con.Query($@"SELECT DISTINCT ap.`appointmentno`,CONCAT(ap.`firstname`,' ',ap.`middlename`,' ',ap.`lastname`) AS fullname,CASE WHEN al.`logevent`='request' THEN 'Processing' WHEN al.logevent='approved' THEN 'Approved' ELSE al.logevent END STATUS,encodedat
                                            FROM ddt_appointment ap JOIN ddt_appointmentlog al ON ap.`appointmentno`= al.`appointmentno` JOIN ddt_proc pr ON ap.`appointmentno`= pr.`appointmentno` WHERE prem_id =@premid LIMIT @offset",
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
        public ResponseModel getAppointmentsRequestTableFinished(PDIagnostics prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT DISTINCT ap.`appointmentno`,CONCAT(ap.`firstname`,' ',ap.`middlename`,' ',ap.`lastname`) AS fullname,CASE WHEN al.`logevent`='request' THEN 'Processing' WHEN al.logevent='approved' THEN 'Approved' ELSE al.logevent END STATUS,encodedat
                                                FROM ddt_appointment ap JOIN ddt_appointmentlog al ON ap.`appointmentno`= al.`appointmentno` JOIN ddt_proc pr ON ap.`appointmentno`= pr.`appointmentno`
                                                WHERE prem_id = @premid AND al.`stsid`= '3' LIMIT @offset",
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
                        var data = con.Query($@"SELECT dps.`description`,da.appointmentno,dp.`resurl`,DATE_FORMAT(da.`finishedat`,'%Y-%m-%d %h:%m %p') finishedat  FROM ddt_appointment da JOIN ddt_proc dp ON da.`appointmentno`=dp.`appointmentno` JOIN ddt_procstatus dps ON dp.`procsts`=dps.`procstsno` WHERE da.`prem_id`=@premid LIMIT @offset",
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
