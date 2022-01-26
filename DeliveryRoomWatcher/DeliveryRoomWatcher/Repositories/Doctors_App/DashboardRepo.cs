using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.Doctors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories.Doctors_App
{
    public class DashboardRepo
    {
        public ResponseModel New_patients(DashboardModel.NewPatients newPatients)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT COUNT(dt.`patno`) new_patients FROM doctortran dt 
	                                  JOIN  inpmaster  ip ON dt.patno = ip.patno
	                                  LEFT JOIN docmaster dm ON dm.doccode = dt.doccode
	                                  LEFT JOIN empmast emp ON CONCAT(emp.lastname,' ,',emp.firstname,' ',emp.middlename)=CONCAT(dm.lastname,' ,',dm.firstname,' ',dm.middlename) 
	                                  LEFT JOIN prem_doctor_usermaster pdu ON pdu.doccode=emp.idno
	                                  LEFT JOIN 
		                                 (SELECT  rt.`patno`, rt.`roomcode`, rt.`bedno`, rt.`nsunit`, rt.roomin, rt.`roomout`, rt.`statustag` FROM roomtran rt 
		                                 WHERE  rt.`roomtype` <> 'P' AND rt.`statustag` IS NULL 
		                                 AND rt.`roomin` = (SELECT MAX(rts.roomin) FROM roomtran rts WHERE rts.patno = rt.`patno` )  
		                                 )  vrt  ON vrt.patno = ip.patno 
		                                 WHERE ip.`dischargedate` IS NULL AND ip.`datecancelled` IS  NULL AND emp.empno =@idno AND ip.admissiondate > DATE_SUB(NOW(), INTERVAL 24 HOUR)", newPatients, transaction: tran);

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
        public ResponseModel Active_patients(DashboardModel.ActivePatients activePatients)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  COUNT(dt.`patno`) active_total_patients FROM doctortran dt 
	                                  JOIN  inpmaster  ip ON dt.patno = ip.patno
	                                  LEFT JOIN docmaster dm ON dm.doccode = dt.doccode
	                                  LEFT JOIN empmast emp ON CONCAT(emp.lastname,' ,',emp.firstname,' ',emp.middlename)=CONCAT(dm.lastname,' ,',dm.firstname,' ',dm.middlename) 
	                                  LEFT JOIN prem_doctor_usermaster pdu ON pdu.doccode=emp.idno
	                                  LEFT JOIN 
		                                 (SELECT  rt.`patno`, rt.`roomcode`, rt.`bedno`, rt.`nsunit`, rt.roomin, rt.`roomout`, rt.`statustag` FROM roomtran rt 
		                                 WHERE  rt.`roomtype` <> 'P' AND rt.`statustag` IS NULL 
		                                 AND rt.`roomin` = (SELECT MAX(rts.roomin) FROM roomtran rts WHERE rts.patno = rt.`patno` )  
		                                 )  vrt  ON vrt.patno = ip.patno 
		                                 WHERE ip.`dischargedate` IS NULL AND ip.`datecancelled` IS  NULL AND emp.empno =@idno ", activePatients, transaction: tran);

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
        public ResponseModel patient_medication(DashboardModel.patient_info patient_Info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM
                                                (SELECT 
                                                pm.`medcode` AS medcode,
                                                pm.`stockcode` AS stockcode,
                                                im.`stockdesc` AS stockdesc,
                                                pm.`dosecode` AS dosecode,
                                                fd.`freqdesc` AS freqdesc,
                                                pm.`datestarted` AS datestarted,
                                                pm.`datestopped` AS datestopped,
                                                pm.`reqdoccode` AS reqdoccode,
                                                pm.`patno` AS patno FROM((patmedication pm JOIN invmaster im ON ((im.`stockcode` = pm.`stockcode`))) 
                                                JOIN freqdosage fd ON ((fd.`freqcode` = pm.`dosecode`)))WHERE pm.patno=@patno) AS tmp ", patient_Info, transaction: tran);

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
        public ResponseModel patient_admission_history(DashboardModel.patient_info patient_Hospitalno)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        //var data = con.Query($@"SELECT inp.`patno`,inp.`admdiagnosis`,inp.`admissiondate` FROM inpmaster inp WHERE inp.`hospitalno`=@hospitalno  AND inp.`dischargedate` IS NOT NULL ORDER BY inp.`admissiondate` ASC ", patient_Hospitalno, transaction: tran);
                        var data = con.Query($@"SELECT * FROM (SELECT i.patno,i.`admdiagnosis`, i.`admissiondate`, i.`dischargedate`, i.`medtype`
                                                ,cs.chiefcomplaint
                                                ,CONCAT(DATEDIFF(COALESCE(i.dischargedate,NOW()), i.admissiondate), 'D ',HOUR(TIMEDIFF (TIME(COALESCE(i.dischargedate,NOW())), TIME(i.admissiondate))),'H ',MINUTE(TIMEDIFF(TIME(COALESCE(i.dischargedate,NOW())), TIME(i.admissiondate) )),'M ') confinement                                         
                                                , vrt.roomcode,vrt.bedno,vrt.nsunit,vrt.roomin,vrt.roomout,vrt.statustag
                                                 FROM inpmaster i 
                                                 LEFT JOIN `clinicalsummary` cs ON i.`patno` = cs.`PatNo`
                                                 LEFT JOIN 
	                                                (SELECT  rt.`patno`, rt.`roomcode`, rt.`bedno`, rt.`nsunit`, rt.roomin, rt.`roomout`, rt.`statustag` FROM `roomtran` rt 
	                                                WHERE  rt.`roomtype` <> 'P' AND rt.`statustag` IS NULL 
	                                                AND rt.`roomin` = (SELECT MAX(rts.roomin) FROM roomtran rts WHERE rts.patno = rt.`patno` )  
	                                                )  vrt  ON vrt.patno = i.patno 
                                                 WHERE i.hospitalno = (SELECT hospitalno FROM inpmaster WHERE    patno=@patno )) AS tmp", patient_Hospitalno, transaction: tran);

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
        public ResponseModel patient_admission_CareTeam(DashboardModel.patient_info patient_Info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  CONCAT(dm.firstname,', ',dm.middlename,', ',dm.lastname,',',dm.doctitle ) AS docname,case when dt.admdoctag='F' then 'Assisting' else 'Attending' end as tag,dt.servicebegin,dt.serviceend,CASE WHEN dt.maindoctor='1' THEN 'YES' ELSE 'NO' END AS maindoctor FROM doctortran dt LEFT JOIN docmaster dm
                                                ON dm.`doccode` = dt.`doccode` WHERE dt.patno = @patno", patient_Info, transaction: tran);

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
        public ResponseModel patient_admission_DietOrders(DashboardModel.patient_info patient_Info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT dor.dkind,CONCAT(dm.firstname,', ',dm.middlename,', ',dm.lastname,',',dm.doctitle ) AS docname,dor.dietdate,dor.foodtowatcher FROM dietorders dor  LEFT JOIN docmaster dm ON dor.reqdoctor=dm.doccode
                                                  WHERE dor.patno=@patno", patient_Info, transaction: tran);

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
        public ResponseModel patient_laboratories(DashboardModel.patient_info patient_Info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM (
	                                    SELECT  ts.patno, td.`resultno`,td.done
	                                    ,opr.`procdesc`,opr.`dateencoded`
                                        ,td.refcode
	                                    FROM trandtls td  
	                                    JOIN prochdr opr ON  td.`refcode` = opr.`proccode`
	                                    JOIN transum ts ON ts.`csno` = td.`csno`
	                                    WHERE patno=@patno AND td.unit = 'proc' AND td.resultno != ''  GROUP BY td.`resultno`) AS tmp ", patient_Info, transaction: tran);

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
        public ResponseModel patient_CourseOntheWard(DashboardModel.patient_info patient_Info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT cwkey,cwdate,`cworder_action`,dateencoded FROM clinsum_cw WHERE patno=@patno", patient_Info, transaction: tran);

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
        public ResponseModel patient_general_info(DashboardModel.patient_info patient_Info)
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
                                          COALESCE(CivilStatus(`ptm`.`cs`),'') AS civilstatus,inp.`nationality`,inp.`religion`,ptm.`birthdate`, inp.mobileno,inp.phoneno,inp.emailadd,inp.admcompaddress,
                                          CONCAT(FLOOR(HOUR(TIMEDIFF(inp.admissiondate,COALESCE(inp.dischargedate,NOW())))/24),'D ',MOD(HOUR(TIMEDIFF(inp.admissiondate,COALESCE(inp.dischargedate,NOW()))),24),'H ',MINUTE(TIMEDIFF(inp.admissiondate,COALESCE(inp.dischargedate,NOW()))),'M') confinement,
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
                                          cl.`ChiefComplaint`   AS complaint,
                                          inp.`admdiagnosis`    AS admdiagnosis,
                                          inp.`medtype`         AS medtype,
                                          inp.dischargedate,
                                          cl.PastHistory,
                                          cl.BriefHistory,
                                          (SELECT CONCAT(BloodPresure,'|',HeartRate,'|',ResRate,'|',Temperature,'|',Height,'|',Weight,'|',BMI) FROM vitalsigns vs WHERE vs.patno =inp.patno ORDER BY DateEncoded DESC LIMIT 1 ) AS vital_signs
                                        FROM inpmaster inp LEFT JOIN patmaster ptm ON ptm.`hospitalno` = inp.`hospitalno`
                                        LEFT JOIN clinicalsummary cl   ON cl.`PatNo` = inp.`patno`
					                    LEFT JOIN region r ON r.`regioncode`=ptm.`perregion`
                                               LEFT JOIN province pr ON pr.`provincecode`=ptm.`perprovince`
                                               LEFT JOIN citymunicipality cm ON cm.`citymuncode`=ptm.`percitymun`
                                               LEFT JOIN barangay br ON br.`barangaycode`=ptm.`perbarangay` 
                                        WHERE inp.`cancelled` = 'N' AND inp.`patno`=@patno", patient_Info, transaction: tran);

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
        public ResponseModel search_patient(DashboardModel.filter_patients search)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var query = $@"SELECT dt.`patno`,ip.`hospitalno`,CONCAT(ip.`admprefix`,'',ip.`admlastname`,' ,',ip.`admfirstname`,' ',ip.`admmiddlename`,' ', ip.`admsuffix`) AS patientname, ip.`admcompaddress`
                                                FROM doctortran dt 
                                                JOIN  inpmaster  ip ON dt.patno = ip.patno
                                                LEFT JOIN docmaster dm ON dm.doccode = dt.doccode
                                                LEFT JOIN empmast emp ON CONCAT(emp.lastname,' ,',emp.firstname,' ',emp.middlename)=CONCAT(dm.lastname,' ,',dm.firstname,' ',dm.middlename) 
                                                LEFT JOIN prem_doctor_usermaster pdu ON pdu.doccode=emp.idno
                                                LEFT JOIN (SELECT  rt.`patno`, rt.`roomcode`, rt.`bedno`, rt.`nsunit`, rt.roomin, rt.`roomout`, rt.`statustag` FROM roomtran rt 
                                                WHERE  rt.`roomtype` <> 'P' AND rt.`statustag` IS NULL 
                                                AND rt.`roomin` = (SELECT MAX(rts.roomin) FROM roomtran rts WHERE rts.patno = rt.`patno` ))  vrt ON vrt.patno = ip.patno 
                                                WHERE ip.`dischargedate` IS NULL AND ip.`datecancelled` IS  NULL AND emp.empno =@idno AND dt.patno LIKE CONCAT('%',@search,'%') 
                                                GROUP BY dt.patno"; 
                     
                        var data = con.Query(query, search, transaction: tran);
                        if (data.Count()==0)
                        {
                            var query2 = $@"SELECT dt.`patno`,ip.`hospitalno`,CONCAT(ip.`admprefix`,'',ip.`admlastname`,' ,',ip.`admfirstname`,' ',ip.`admmiddlename`,' ', ip.`admsuffix`) AS patientname, ip.`admcompaddress`
                                                FROM doctortran dt 
                                                JOIN  inpmaster  ip ON dt.patno = ip.patno
                                                LEFT JOIN docmaster dm ON dm.doccode = dt.doccode
                                                LEFT JOIN empmast emp ON CONCAT(emp.lastname,' ,',emp.firstname,' ',emp.middlename)=CONCAT(dm.lastname,' ,',dm.firstname,' ',dm.middlename) 
                                                LEFT JOIN prem_doctor_usermaster pdu ON pdu.doccode=emp.idno
                                                LEFT JOIN (SELECT  rt.`patno`, rt.`roomcode`, rt.`bedno`, rt.`nsunit`, rt.roomin, rt.`roomout`, rt.`statustag` FROM roomtran rt 
                                                WHERE  rt.`roomtype` <> 'P' AND rt.`statustag` IS NULL 
                                                AND rt.`roomin` = (SELECT MAX(rts.roomin) FROM roomtran rts WHERE rts.patno = rt.`patno` ))  vrt ON vrt.patno = ip.patno 
                                                WHERE ip.`dischargedate` IS NULL AND ip.`datecancelled` IS  NULL AND emp.empno =@idno AND CONCAT(ip.`admprefix`,'',ip.`admlastname`,' ,',ip.`admfirstname`,' ',ip.`admmiddlename`,' ', ip.`admsuffix`) LIKE CONCAT('%',@search,'%') 
                                                GROUP BY dt.patno";

                            var data2= con.Query(query2, search, transaction: tran);
                            return new ResponseModel
                            {
                                success = true,
                                data = data2
                            };
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = true,
                                data = data
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
