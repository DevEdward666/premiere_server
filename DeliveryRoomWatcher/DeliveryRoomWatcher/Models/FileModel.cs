using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class FileModel
    {
        public string filename { get; set; }
        public string filepath { get; set; }
        public string filetype { get; set; }
        public string fileid { get; set; }  
        public string title { get; set; }
        public string description { get; set; }


        public class Create_New_File {
            public string description { get; set; }
            public string title { get; set; }
            public string filename { get; set; }
            public string filepath { get; set; }
            public string filetype { get; set; }
            public string master_file_id { get; set; }

        }
        public class Search_File
        {
            public string title { get; set; }

        }

    }
}
