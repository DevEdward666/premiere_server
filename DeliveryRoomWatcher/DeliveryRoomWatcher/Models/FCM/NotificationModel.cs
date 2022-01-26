using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.FCM
{
    public class NotificationModel
    {
        [JsonProperty("deviceId")]
        public List<string> DeviceId { get; set; }
        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }


        public class inserttoken
        {
            [JsonProperty("user_id")]
            public string user_id { get; set; }
            [JsonProperty("token")]
            public string token { get; set; }
            [JsonProperty("phone_brand")]
            public string phone_brand { get; set; }
            [JsonProperty("phone_model")]
            public string phone_model { get; set; }
        }
    }

    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
        }
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataPayload Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }
}
