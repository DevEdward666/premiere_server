using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Entities;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
 
    public class ServicesRepo
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        public ResponseModel getServices(PServices.PGetServices offset)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_services LIMIT @offset",
                          offset, transaction: tran
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
        public ResponseModel getservicesdesc(PServices.GetServiceID service)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_services_desc WHERE serv_id=@service_id",
                          service, transaction: tran
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
        public ResponseModel InsertNewServices(ServicesModel.Create_Services create_services)
        {
           
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        string AnnouncementId = $@"SELECT NextServicesId()";
                        string getAnnouncementId = con.QuerySingleOrDefault<string>(AnnouncementId, create_services, transaction: tran);
                        create_services.services_id = getAnnouncementId;
                        create_services.services_id = getAnnouncementId;

                        if (getAnnouncementId != null)
                        {
                        
                           

                                    FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new ServicesFileEntity()
                                    {
                                        services_id = getAnnouncementId
                                    };

                                    file_upload_response = UseLocalFiles.UploadLocalFile(create_services.attach_files, $@"Resources\\Services\\admin\\Services\\{create_services.services_id}\\", create_services.services_id);
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
                                        proc_file_payload.file_dest = $@"Resources\\Services\\admin\\Services\\{create_services.services_id}\\{file_upload_response.data.name}";
                                        proc_file_payload.file_name = file_upload_response.data.name;
                                    }


                                    int add_file = con.Execute($@"INSERT INTO prem_services SET id=@services_id,services_img='{proc_file_payload.file_dest}', hosp_serv_name=@hosp_serv_name,hosp_serv_description=@hosp_serv_description,createdAt=NOW()",
                                        create_services, transaction: tran);

                                    if (add_file <= 0)
                                    {
                                        tran.Rollback();
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                                        };
                                    }
                                

                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Services added successfully"
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
        public ResponseModel UpdateNewServices(ServicesModel.Create_Services create_services)
        {
           
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        FileResponseModel file_upload_response = new FileResponseModel
                                    {
                                        success = true
                                    };

                                    var proc_file_payload = new ServicesFileEntity()
                                    {
                                        services_id = create_services.services_id
                                    };

                                    file_upload_response = UseLocalFiles.UploadLocalFile(create_services.attach_files, $@"Resources\\Services\\admin\\Services\\{create_services.services_id}\\", create_services.services_id);
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
                                        proc_file_payload.file_dest = $@"Resources\\Services\\admin\\Services\\{create_services.services_id}\\{file_upload_response.data.name}";
                                        proc_file_payload.file_name = file_upload_response.data.name;
                                    }
                                    
                                    int update_file = con.Execute($@"UPDATE prem_services SET services_img='{proc_file_payload.file_dest}',hosp_serv_name=@hosp_serv_name,hosp_serv_description=@hosp_serv_description WHERE id=@services_id",
                                        create_services, transaction: tran);

                                    if (update_file <= 0)
                                    {
                                        tran.Rollback();
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                                        };
                                    }
                                

                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Services added successfully"
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
        public ResponseModel UpdateNewServicesWithoutImage(ServicesModel.Create_Services create_services)
        {
           
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int update_file = con.Execute($@"UPDATE prem_services SET hosp_serv_name=@hosp_serv_name,hosp_serv_description=@hosp_serv_description WHERE id=@services_id",
                                        create_services, transaction: tran);

                                    if (update_file <= 0)
                                    {
                                        tran.Rollback();
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                                        };
                                    }
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Services added successfully"
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
        public ResponseModel InsertNewServicesDesc(ServicesModel.Create_Services_desc create_services)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        string Service_descId = $@"SELECT NextServiceDescId()";
                        string getServiceDesc_id = con.QuerySingleOrDefault<string>(Service_descId, create_services, transaction: tran);
                        create_services.serv_desc_id = getServiceDesc_id;
                        create_services.serv_desc_id = getServiceDesc_id;

                        if (getServiceDesc_id != null)
                        {



                            FileResponseModel file_upload_response = new FileResponseModel
                            {
                                success = true
                            };

                            var proc_file_payload = new ServicesFileEntity()
                            {
                                services_id = getServiceDesc_id
                            };

                            file_upload_response = UseLocalFiles.UploadLocalFile(create_services.attach_files, $@"Resources\\Services\\admin\\Services\\ServiceDesc\\{create_services.services_id}\\", create_services.services_id);
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
                                proc_file_payload.file_dest = $@"Resources\\Services\\admin\\Services\\ServiceDesc\\{create_services.services_id}\\{file_upload_response.data.name}";
                                proc_file_payload.file_name = file_upload_response.data.name;
                            }


                            int add_file = con.Execute($@"INSERT INTO prem_services_desc SET serv_desc_id=@serv_desc_id,serv_id=@services_id,images='{proc_file_payload.file_dest}',title=@title,`desc`=@desc,encoded=NOW()",
                                create_services, transaction: tran);

                            if (add_file <= 0)
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                                };
                            }


                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Services Description added successfully"
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
        public ResponseModel UpdateServicesDesc(ServicesModel.Create_Services_desc create_services)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                            FileResponseModel file_upload_response = new FileResponseModel
                            {
                                success = true
                            };

                            var proc_file_payload = new ServicesFileEntity()
                            {
                                services_id = create_services.serv_desc_id
                            };

                            file_upload_response = UseLocalFiles.UploadLocalFile(create_services.attach_files, $@"Resources\\Services\\admin\\Services\\ServiceDesc\\{create_services.services_id}\\", create_services.services_id);
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
                                proc_file_payload.file_dest = $@"Resources\\Services\\admin\\Services\\ServiceDesc\\{create_services.services_id}\\{file_upload_response.data.name}";
                                proc_file_payload.file_name = file_upload_response.data.name;
                            }


                            int update_file = con.Execute($@"UPDATE prem_services_desc SET images='{proc_file_payload.file_dest}',title=@title,`desc`=@desc WHERE serv_desc_id=@serv_desc_id",
                                create_services, transaction: tran);



                            if (update_file <= 0)
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                                };
                            }


                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Services Description added successfully"
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
      
        public ResponseModel UpdateServicesDescWithoutImage(ServicesModel.Create_Services_desc create_services)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                           
                            int update_file = con.Execute($@"UPDATE prem_services_desc SET title=@title,`desc`=@desc WHERE serv_desc_id=@serv_desc_id",
                                create_services, transaction: tran);



                            if (update_file <= 0)
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = false,
                                    message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                                };
                            }


                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Services Description added successfully"
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
        public ResponseModel UpdateServicesInfo(ServicesModel.Create_Services_info create_services)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        FileResponseModel file_upload_response = new FileResponseModel
                        {
                            success = true
                        };

                        var proc_file_payload = new ServicesFileEntity()
                        {
                            services_id = create_services.services_id
                        };

                        file_upload_response = UseLocalFiles.UploadLocalFile(create_services.attach_files, $@"Resources\\Services\\admin\\Services\\ServiceDesc\\{create_services.services_id}\\", create_services.services_id);
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
                            proc_file_payload.file_dest = $@"Resources\\Services\\admin\\Services\\ServiceDesc\\{create_services.services_id}\\{file_upload_response.data.name}";
                            proc_file_payload.file_name = file_upload_response.data.name;
                        }


                        int update_file = con.Execute($@"UPDATE prem_services_info SET serv_image='{proc_file_payload.file_dest}',serv_title=@subject,serv_desc=@description WHERE serv_desc_id=@services_id",
                            create_services, transaction: tran);



                        if (update_file <= 0)
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {
                                success = false,
                                message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                            };
                        }


                        tran.Commit();
                        return new ResponseModel
                        {
                            success = true,
                            message = "Services Info added successfully"
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
        public ResponseModel UpdateServicesInfoWithoutImage(ServicesModel.Create_Services_info create_services)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        int update_file = con.Execute($@"UPDATE prem_services_info SET serv_title=@subject,serv_desc=@description WHERE serv_desc_id=@services_id",
                            create_services, transaction: tran);



                        if (update_file <= 0)
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {
                                success = false,
                                message = $"The {create_services.attach_files.FileName} could not be saved! Please try again!"
                            };
                        }


                        tran.Commit();
                        return new ResponseModel
                        {
                            success = true,
                            message = "Services Description added successfully"
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
        public ResponseModel getservicesinfo(PServices.GetServiceDescID service)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_services_info WHERE serv_desc_id=@service_desc_id",
                          service, transaction: tran
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
