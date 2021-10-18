using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Common
{
    public class FileModel
    {
      
        [FromForm(Name = "FolderName")]
        public string FolderName{ get; set; }
        [FromForm(Name = "FileName")]
        public string FileName { get; set; }
        [FromForm(Name = "FormFile")]
        public IFormFile FormFile { get; set; }
        public string docs { get; set; }
        public string img { get; set; }
        public string username { get; set; }
    }
    public class GetFileModel
    {

        [FromForm(Name = "FileName")]
        public string FileName { get; set; }
    }
    public class setNewImage
    {
        public string news_id { get; set; }
        public string base64 { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public List<IFormFile> image { get; set; }
    }
    public class FileModelProfile
    {
        [FromForm(Name = "FolderName")]
        public string FolderName { get; set; }
        [FromForm(Name = "FileName")]
        public string FileName { get; set; }
        [FromForm(Name = "FormFile")]
        public IFormFile FormFile { get; set; }
        public string docs { get; set; }
        public string img { get; set; }
        public string username { get; set; }
    }
}
