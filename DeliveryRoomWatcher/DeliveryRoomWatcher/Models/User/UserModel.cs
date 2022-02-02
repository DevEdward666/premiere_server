using DeliveryRoomWatcher.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.User
{
    public class UserModel
    {
        public string username { get; set; }
        public string empname { get; set; }
        public string modid { get; set; }
        public string encodedat { get; set; }
        public string encodedby { get; set; }
        public ResponseModel resposen { get; set; }
        public class setUserImage
        {
            public string user_id { get; set; }
            public List<IFormFile> user_image { get; set; }
        }      
        public class getoken
        {
            public string user_id { get; set; }
        }
        public class getCurrentOtp
        {
            public string username { get; set; }
            public string otp { get; set; }
        }
    }
}
