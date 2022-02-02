using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Entities;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.Employee;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories.Employee
{
    public class MemoRepo
    {
        public ResponseModel getallmemo()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT  pm.id,pm.To,pm.From,pm.Subject,pm.Description,pma.file_dest,pmd.deptcode,pm.published FROM prem_memo pm LEFT JOIN prem_memo_attachments pma ON pm.id=pma.memo_id LEFT JOIN prem_memo_departments pmd ON pm.id=pmd.memo_id GROUP BY pm.id", null, transaction: tran);

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
        public ResponseModel getmemoFilter(MemoModel.FilterMemo getmemo)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string query = "";
                        if (getmemo.published != "" && getmemo.deptcode != null && getmemo.month !="" && getmemo.year != "")
                        {
                            query = $@"SELECT pm.id,pm.To,pm.From,pm.Subject,pm.Description,pma.file_dest,pmd.deptcode FROM prem_memo pm left JOIN prem_memo_attachments pma ON pm.id=pma.memo_id left JOIN prem_memo_departments pmd ON pm.id=pmd.memo_id WHERE pmd.deptcode = @deptcode OR pm.published=@published OR MONTH(pm.createdDate)=@month AND YEAR(pm.createdDate)=@year GROUP BY pm.id";
                        }else if(getmemo.published != ""  && getmemo.month != "" && getmemo.year != "")
                        {
                            query = $@"SELECT pm.id,pm.To,pm.From,pm.Subject,pm.Description,pma.file_dest,pmd.deptcode FROM prem_memo pm left JOIN prem_memo_attachments pma ON pm.id=pma.memo_id left  JOIN prem_memo_departments pmd ON pm.id=pmd.memo_id WHERE pm.published=@published and MONTH(pm.createdDate)=@month AND YEAR(pm.createdDate)=@year GROUP BY pm.id";
                        }else if (getmemo.published != "" && getmemo.deptcode != null )
                        {
                            query = $@"SELECT pm.id,pm.To,pm.From,pm.Subject,pm.Description,pma.file_dest,pmd.deptcode FROM prem_memo pm left JOIN prem_memo_attachments pma ON pm.id=pma.memo_id left JOIN prem_memo_departments pmd ON pm.id=pmd.memo_id WHERE pmd.deptcode = @deptcode and pm.published=@published  GROUP BY pm.id";
                        }   
                        else if (getmemo.published != ""  )
                        {
                            query = $@"SELECT pm.id,pm.To,pm.From,pm.Subject,pm.Description,pma.file_dest,pmd.deptcode FROM prem_memo pm left JOIN prem_memo_attachments pma ON pm.id=pma.memo_id left JOIN prem_memo_departments pmd ON pm.id=pmd.memo_id WHERE pm.published=@published  GROUP BY pm.id";
                        }
                            var data = con.Query(query, getmemo, transaction: tran);
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
        public ResponseModel getmemo(MemoModel.GetMemo getmemo)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pm.id,pm.To,pm.From,pm.Subject,pm.Description,pma.file_dest,pmd.deptcode FROM prem_memo pm JOIN prem_memo_attachments pma ON pm.id=pma.memo_id JOIN prem_memo_departments pmd ON pm.id=pmd.memo_id WHERE pmd.deptcode=@deptcode GROUP BY pm.id", getmemo, transaction: tran);

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
        public ResponseModel getmemoAttachMents(MemoModel.GetMemoAttachments getmemo)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_memo_attachments WHERE memo_id=@memo_id", getmemo, transaction: tran);

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

        public ResponseModel InsertNewMemo(MemoModel.Create_Memo create_memo)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        string AnnouncementId = $@"SELECT NextMemoId()";
                        string getMemoId = con.QuerySingleOrDefault<string>(AnnouncementId, create_memo, transaction: tran);
                        create_memo.memo_id = getMemoId;
                        create_memo.memo_id = getMemoId;

                        if (getMemoId != null)
                        {

                            int insert_appointment = con.Execute($@"INSERT INTO prem_memo SET id=@memo_id,`To`=@to,`From`=@from,`Subject`=@subject,Description=@description,department=@department,published=true,createdDate=NOW()",
                                               create_memo, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                               
                                    foreach (var f in create_memo.attach_files)
                                    {

                                        FileResponseModel file_upload_response = new FileResponseModel
                                        {
                                            success = true
                                        };

                                        var proc_file_payload = new MemoFileEntity()
                                        {
                                            memo_id = getMemoId
                                        };

                                        file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources/Announcements/{create_memo.memo_id}/", create_memo.memo_id);
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
                                            proc_file_payload.file_dest = $@"Resources/Announcements/{create_memo.memo_id}/{file_upload_response.data.name}";
                                            proc_file_payload.file_name = file_upload_response.data.name;
                                        }


                                        int add_file = con.Execute(@"INSERT INTO `prem_memo_attachments` SET memo_id=@memo_id, file_dest=@file_dest,file_name=@file_name,createdDate=NOW();",
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
                                if (create_memo.listofdepartment!=null)
                                {
                                    foreach (var dept in create_memo.listofdepartment)
                                    {
                                        int i = create_memo.listofdepartment.Count;
                                        int x = 0;
                                        int insert_department = con.Execute($@"INSERT INTO prem_memo_departments SET memo_id='{getMemoId}',deptcode=@deptcode,deptname=@deptname,createdDate=NOW()", dept, transaction: tran);
                                        if (insert_department <= 0)
                                        {
                                            tran.Rollback();
                                            response.success = false;
                                            response.message = "Error! Insertion of Log Failed.";

                                        }

                                    }
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Successfully Published"
                                    };


                                }

                                tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Successfully Published"
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
                                message = "No affected rows when trying to save the consultation request. "
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
        public ResponseModel UpdateMemoWithFiles (MemoModel.Create_Memo update_memo)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int insert_appointment = con.Execute($@"UPDATE prem_memo SET `To`=@to,`From`=@from,`Subject`=@subject,Description=@description WHERE id=@memo_id",
                                               update_memo, transaction: tran);
                        int deletefiles = con.Execute($@"DELETE FROM prem_memo_attachments WHERE memo_id=@memo_id",
                                              update_memo, transaction: tran);
                       
                        if (insert_appointment >= 0 && deletefiles>=0 )
                            {
                               
                                    foreach (var f in update_memo.attach_files)
                                    {

                                        FileResponseModel file_upload_response = new FileResponseModel
                                        {
                                            success = true
                                        };

                                        var proc_file_payload = new MemoFileEntity()
                                        {
                                            memo_id = update_memo.memo_id
                                        };

                                        file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources/Announcements/{update_memo.memo_id}/", update_memo.memo_id);
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
                                            proc_file_payload.file_dest = $@"Resources/Announcements/{update_memo.memo_id}/{file_upload_response.data.name}";
                                            proc_file_payload.file_name = file_upload_response.data.name;
                                        }


                                        int add_file = con.Execute(@"INSERT INTO `prem_memo_attachments` SET memo_id=@memo_id, file_dest=@file_dest,file_name=@file_name,createdDate=NOW();",
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
                              

                                tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Successfully Published"
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
        public ResponseModel UpdateMemoWithDepartment (MemoModel.Create_Memo update_memo)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int insert_appointment = con.Execute($@"UPDATE prem_memo SET `To`=@to,`From`=@from,`Subject`=@subject,Description=@description WHERE id=@memo_id",
                                               update_memo, transaction: tran);
                    
                        int deletedepartments = con.Execute($@"DELETE FROM prem_memo_departments WHERE memo_id=@memo_id",
                                              update_memo, transaction: tran);
                        var memo_id = update_memo.memo_id;
                        if (insert_appointment >= 0  && deletedepartments>=0)
                            {   
                                if (update_memo.listofdepartment!=null)
                                {
                                    foreach (var dept in update_memo.listofdepartment)
                                    {
                                        int i = update_memo.listofdepartment.Count;
                                        int x = 0;
                                        int insert_department = con.Execute($@"INSERT INTO prem_memo_departments SET memo_id='{memo_id}',deptcode=@deptcode,deptname=@deptname,createdDate=NOW()", dept, transaction: tran);
                                        if (insert_department <= 0)
                                        {
                                            tran.Rollback();
                                            response.success = false;
                                            response.message = "Error! Insertion of Log Failed.";

                                        }

                                    }
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Updated Successfully"
                                    };


                                }

                                tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Successfully Published"
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
        public ResponseModel UpdateMemoWithFilesandDepartment (MemoModel.Create_Memo update_memo)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int insert_appointment = con.Execute($@"UPDATE prem_memo SET `To`=@to,`From`=@from,`Subject`=@subject,Description=@description WHERE id=@memo_id",update_memo, transaction: tran);
                        int deletefiles = con.Execute($@"DELETE FROM prem_memo_attachments WHERE memo_id=@memo_id", update_memo, transaction: tran);
                        int deletedepartments = con.Execute($@"DELETE FROM prem_memo_departments WHERE memo_id=@memo_id", update_memo, transaction: tran);
                        if (insert_appointment >= 0 && deletefiles>=0 && deletedepartments>=0)
                            {
                               
                                    foreach (var f in update_memo.attach_files)
                                    {

                                        FileResponseModel file_upload_response = new FileResponseModel
                                        {
                                            success = true
                                        };

                                        var proc_file_payload = new MemoFileEntity()
                                        {
                                            memo_id = update_memo.memo_id
                                        };

                                        file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources/Announcements/{update_memo.memo_id}/", update_memo.memo_id);
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
                                            proc_file_payload.file_dest = $@"Resources/Announcements/{update_memo.memo_id}/{file_upload_response.data.name}";
                                            proc_file_payload.file_name = file_upload_response.data.name;
                                        }


                                        int add_file = con.Execute(@"INSERT INTO `prem_memo_attachments` SET memo_id=@memo_id, file_dest=@file_dest,file_name=@file_name,createdDate=NOW();",
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
                                if (update_memo.listofdepartment!=null)
                                {
                                    foreach (var dept in update_memo.listofdepartment)
                                    {
                                        int i = update_memo.listofdepartment.Count;
                                        int x = 0;
                                    var memo_id = update_memo.memo_id;
                                    int insert_department = con.Execute($@"INSERT INTO prem_memo_departments SET memo_id='{memo_id}',deptcode=@deptcode,deptname=@deptname,createdDate=NOW()", dept, transaction: tran);
                                        if (insert_department <= 0)
                                        {
                                            tran.Rollback();
                                            response.success = false;
                                            response.message = "Error! Insertion of Log Failed.";

                                        }

                                    }
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Updated Successfully"
                                    };


                                }

                                tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Successfully Published"
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
        public ResponseModel UpdateMemo(MemoModel.Update_Memo update_memo)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        

                            int insert_appointment = con.Execute($@"UPDATE prem_memo SET `To`=@to,`From`=@from,`Subject`=@subject,Description=@description WHERE id=@memo_id",
                                               update_memo, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                               
                               

                                tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Updated Successfully"
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
        public ResponseModel UpdateMemoUnpublished(MemoModel.Update_Memo_Published update_memo)
        {
            ResponseModel response = new ResponseModel();
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        

                            int insert_appointment = con.Execute($@"UPDATE prem_memo SET published=@published WHERE id=@memo_id",
                                               update_memo, transaction: tran);
                            if (insert_appointment >= 0)
                            {
                               
                               

                                tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Memo Updated Successfully"
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
