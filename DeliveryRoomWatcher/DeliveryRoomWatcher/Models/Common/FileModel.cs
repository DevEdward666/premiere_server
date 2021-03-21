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
    }
    public class GetFileModel
    {

        [FromForm(Name = "FileName")]
        public string FileName { get; set; }
    }
    public class setNewImage
    {
        [Display(Name = "newsImage")]
        public string base64 { get; set; }
        [Display(Name = "newsTitle")]
        public string title { get; set; }
        [Display(Name = "newsDescription")]
        public string description { get; set; }
        [Display(Name = "newsAuthor")]
        public string author { get; set; }
        [Display(Name = "Image")]
        public string image { get; set; }
    }
    public class FileModelProfile
    {
        [FromForm(Name = "FolderName")]
        public string FolderName { get; set; }
        [FromForm(Name = "FileName")]
        public string FileName { get; set; }
        [FromForm(Name = "FormFile")]
        public IFormFile FormFile { get; set; }
    }
}
