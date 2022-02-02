using ExecutiveAPI.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Model
{
    public class UserModel
    {
        public class AuthUser
        {
        public string username { get; set; }
        public string password { get; set; }
        }
        public class Users
        {
            public string username { get; set; }
            public string empname { get; set; }
            public string modid { get; set; }
            public string encodedat { get; set; }
            public string encodedby { get; set; }
            public ResponseModel resposen { get; set; }
        }
      
    }
}
