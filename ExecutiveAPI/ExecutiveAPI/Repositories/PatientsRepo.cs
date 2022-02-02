using Dapper;
using ExecutiveAPI.Config;
using ExecutiveAPI.Model.Common;
using ExecutiveAPI.Model.PatientsModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Repositories
{
    public class PatientsRepo
    {
        public ResponseModel gettotalpatientDefault()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT COUNT(patno) AS Total_Patient,YEAR(admissiondate) AS YEAR ,monthtowords(MONTH(admissiondate))AS MONTH FROM inpmaster  WHERE YEAR(admissiondate) =YEAR(CURDATE())  
                                                GROUP BY YEAR(admissiondate),MONTH(admissiondate) ORDER BY YEAR(admissiondate),MONTH(admissiondate)",
                          null, transaction: tran
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
        public ResponseModel getpatientcomparedfivedays()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  COUNT(patno) today,(SELECT COUNT(patno) AS Total_Patient FROM inpmaster  WHERE admissiondate >= DATE(NOW()) - INTERVAL 5 DAY) AS last_5_days FROM inpmaster  
                                                WHERE admissiondate >= DATE(NOW())",
                          null, transaction: tran
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
        public ResponseModel gettotalpatientOptions(PatientModel.GetPatientOptions options)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string Query = "";
                        if (options.month != "" && options.year != "")
                        {
                            Query = $@"SELECT COUNT(patno) AS Total_Patient,YEAR(admissiondate) AS YEAR ,monthtowords(MONTH(admissiondate))AS MONTH FROM inpmaster  WHERE YEAR(admissiondate) =@year AND MONTH(admissiondate)=@month  
                                     GROUP BY YEAR(admissiondate),MONTH(admissiondate) ORDER BY YEAR(admissiondate),MONTH(admissiondate)";

                        }
                        else if (options.year != "")
                        {
                            Query = $@"SELECT COUNT(patno) AS Total_Patient,YEAR(admissiondate) AS YEAR ,monthtowords(MONTH(admissiondate))AS MONTH FROM inpmaster  WHERE YEAR(admissiondate) =@year  
                                    GROUP BY YEAR(admissiondate),MONTH(admissiondate) ORDER BY YEAR(admissiondate),MONTH(admissiondate)";

                        }
                        else if (options.month != "")
                        {
                            Query = $@"SELECT COUNT(patno) AS Total_Patient,YEAR(admissiondate) AS YEAR ,monthtowords(MONTH(admissiondate))AS MONTH FROM inpmaster WHERE MONTH(admissiondate)=@month
                                    GROUP BY YEAR(admissiondate),MONTH(admissiondate) ORDER BY YEAR(admissiondate),MONTH(admissiondate)";

                        }
                        else
                        {
                            Query = $@"SELECT COUNT(patno) AS Total_Patient,YEAR(admissiondate) AS YEAR ,monthtowords(MONTH(admissiondate))AS MONTH FROM inpmaster  WHERE YEAR(admissiondate) =YEAR(CURDATE())   
                                     GROUP BY YEAR(admissiondate),MONTH(admissiondate) ORDER BY YEAR(admissiondate),MONTH(admissiondate)";
                        }

                        var data = con.Query(Query, options, transaction: tran
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
