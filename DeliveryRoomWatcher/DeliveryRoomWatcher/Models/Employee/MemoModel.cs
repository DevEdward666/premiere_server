using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Employee
{
    public class MemoModel
    {
        public class Update_Memo_Published
        {
            public string published { get; set; }
            public string memo_id { get; set; }

        }   
        public class Update_Memo
        {
            public string memo_id { get; set; }
            public string to { get; set; }
            public string from { get; set; }
            public string subject { get; set; }
            public string description { get; set; }
        }     
        public class Create_Memo
        {
            public string memo_id { get; set; }
            public string to { get; set; }
            public string from { get; set; }
            public string subject { get; set; }
            public string description { get; set; }
            public string department { get; set; }
            public string published { get; set; }
            public List<IFormFile> attach_files { set; get; }
            public List<CreateMemoDept> listofdepartment {get;set;}
        }
        public class CreateMemoDept
        {
            public string deptcode { get; set; }
            public string deptname { get; set; }
        }   
        public class FilterMemo
        {
            public string deptcode { get; set; }
            public string month { get; set; }
            public string year { get; set; }
            public string published { get; set; }
        }  
        public class GetMemo
        {
            public string deptcode { get; set; }
        }  
        public class GetMemoAttachments
        {
            public string memo_id { get; set; }
        }
    }
}
