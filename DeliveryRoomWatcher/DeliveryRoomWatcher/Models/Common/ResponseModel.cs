using DeliveryRoomWatcher.Models.Clinic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DeliveryRoomWatcher.Models.PaymongoModel;

namespace DeliveryRoomWatcher.Models.Common
{
    public class ResponseModel
    {
        [Required]
        public bool success { get; set; } = false;
        public object data { get; set; }
        public object data2{ get; set; }
        public string message { get; set; }
        public string other_info { get; set; }
        public List<ErrorModel> errors { get; set; }
        public string file { get; set; }
        public List<PaymongoResouceError> paymongo_errors { get; set; }

    }
    public class News_model
    {
        public string id { get; set; }
        public string Title { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string dateEncoded { get; set; }
        public string likes { get; set; }

        public List<News_model_images> images { get; set; }
}
    public class News_model_images
    {
        public string news_image { get; set; }
        public string news_image_name { get; set; }
    }
    public class UserResponseModel
    {
        [Required]
        public bool success { get; set; } = false;
        public List<UserEntity> data { get; set; }
        public string message { get; set; }
    }

    public class FileModels
    {
        public string name { get; set; }
        public string path { get; set; }
        public string ext { get; set; }
    }
    public class FileResponseModel
    {
        [Required]
        public bool success { get; set; } = false;
        public FileModels data { get; set; }
        public string message { get; set; }
    }

    public class ErrorModel
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
