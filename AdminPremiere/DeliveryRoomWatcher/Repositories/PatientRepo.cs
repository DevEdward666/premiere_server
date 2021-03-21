using AdminPremiere.Parameters;
using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPremiere.Repositories
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
        public ResponseModel getUserInfo(PUsers.PGetUsersInfo prem)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pu.img,pu.docs,pu.prem_id,pu.email,pu.mobileno,pu.birthdate,pum.fullname,r.regiondesc,cm.citymundesc,p.provincedesc,b.barangaydesc,CASE WHEN pu.gender='M' THEN 'Male' ELSE 'Female' END gender  FROM prem_usersinfo pu 
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
    }
}
