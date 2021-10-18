using DeliveryRoomWatcher.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Hooks
{
    public class UseLocalFiles
    {
        public static FileResponseModel UploadLocalFile(IFormFile file, string file_path,string foldername)
        {
            try
            {
                if (file != null)
                {
                    string root_file_name = Path.GetFileName(file.FileName);
                    string ext = Path.GetExtension(root_file_name);

                    string unique_file_name = Path.GetFileNameWithoutExtension(root_file_name)
                              + "_"
                              + Guid.NewGuid().ToString().Substring(0, 4)
                              + ext;

                    string path = Path.Combine(Directory.GetCurrentDirectory(), file_path).Replace("\\", "/");
                 
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        string filepath = Path.Combine(Directory.GetCurrentDirectory(),file_path+"/"+ unique_file_name).Replace("\\", "/");
                        string savefilepath = file_path + unique_file_name.Replace("\\", "/");
                        using (Stream stream = new FileStream(filepath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }


                        return new FileResponseModel
                        {
                            success = true,
                            data = new FileModels
                            {
                                name = unique_file_name,
                                path = savefilepath,
                                ext = ext
                            }
                        };


                    }
                    else
                    {
                        string filepath = Path.Combine(Directory.GetCurrentDirectory(), file_path + "/" + unique_file_name).Replace('\\', '/');
                        string savefilepath = file_path  + unique_file_name.Replace("\\", "/");
                        using (Stream stream = new FileStream(filepath, FileMode.Create))
                        {
                           file.CopyTo(stream);
                        }

                        return new FileResponseModel
                        {
                            success = true,
                            data = new FileModels
                            {
                                name = unique_file_name,
                                path = savefilepath,
                                ext = ext
                            }
                        };

                    }


                }


                return new FileResponseModel
                {
                    success = false,
                    message = "The file cannot be null"
                };
            }
            catch (Exception e)
            {
                return new FileResponseModel
                {
                    success = false,
                    message = e.Message
                };
            }

        }
    }
}
