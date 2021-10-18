using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Clinic;
using DeliveryRoomWatcher.Models.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class ClinicRepo
    {
        public ResponseModel geConsultRequestDtls(consultation_table clinicmodels)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT sm.`sts_desc`,
                                                cr.consult_req_pk as appointment_id, pu.prem_id, pu.firstname, pu.middlename, pu.lastname, pu.suffix, pu.gender, pu.civil_status, pu.nationality_code, pu.religion_code, pu.birthdate, pu.email,
                                                pu.mobileno, pu.address AS line1, pu.barangay_code, pu.province_code, city.citymundesc, dg.regiondesc, CONCAT(
                                                 db.barangaydesc, ',', city.citymundesc, ',', dp.provincedesc) AS line2, pu.zipcode, NOW() AS requested_at, cr.`sts_pk` AS STATUS 
                                                FROM prem_usersinfo pu JOIN prem_usermaster pum ON pu.username = pum.username JOIN ddt_province dp ON dp.provincecode = pu.province_code 
                                                JOIN ddt_barangay db ON db.barangaycode = pu.barangay_code JOIN citymunicipality city ON city.citymuncode = pu.city_code 
                                                JOIN ddt_region dg  ON dg.regioncode = pu.region_code  JOIN consult_request cr ON cr.`prem_id`=pu.prem_id 
                                                LEFT JOIN bill_paymongo bp ON bp.`id`=cr.`paymongo_src_id` JOIN status_master sm ON sm.`sts_pk`=cr.sts_pk
                                                WHERE pu.prem_id =@premid AND cr.consult_req_pk=@consult_req_pk ", clinicmodels, transaction: tran);

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
        public ResponseModel geConsultRequestTable(consultation_table clinicmodels)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT cr.`consult_req_pk`,prem_id,  CONCAT(cr.`first_name`,' ',cr.`last_name`)AS fullname,cr.hospital_no,bp.`event_type`,cr.`paymongo_paid_at`,sm.`sts_desc`,sm.sts_color,sm.`sts_bg_color` 
                                                FROM consult_request cr LEFT JOIN bill_paymongo bp ON cr.`paymongo_src_id`=bp.`id` 
                                                JOIN status_master sm ON sm.`sts_pk`=cr.`sts_pk` where prem_id=@premid and sts_desc=@status  group by cr.consult_req_pk ORDER BY consult_req_pk desc limit @offset ", clinicmodels, transaction: tran);

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
