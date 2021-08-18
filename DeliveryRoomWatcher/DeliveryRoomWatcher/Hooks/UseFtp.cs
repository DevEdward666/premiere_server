using DeliveryRoomWatcher.Models;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace DeliveryRoomWatcher.Hooks
{
    public class UseFtp
    {
        public static List<FtpModel> GetFtpFiles(string ftp_url, int ftp_port, string file_path, string username, string password)
        {
            var files = new List<FtpModel>();

            FtpClient client = new FtpClient(ftp_url, ftp_port, username, password);
            client.Connect();

            foreach (FtpListItem item in client.GetListing(file_path))
            {
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    files.Add(new FtpModel
                    {
                        file_name = item.Name,
                        modified_time = client.GetModifiedTime(item.FullName),
                        file_path = "ftp://" + ftp_url + ":" + ftp_port + item.FullName,
                    });
                }
            }
            return files;
        }
        public static byte[] DownloadFtp(string ftp_path, string username, string password)
        {
            try
            {
                var request = new WebClient();
                request.Credentials = new NetworkCredential(username, password);
                var byte_file = request.DownloadData(ftp_path);

                return byte_file;
            }
            catch (Exception err)
            {
                return null;
            }

        }
    }
}
