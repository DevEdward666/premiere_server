using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.User
{
    public class mdlMessagePost
    {
        public class MessagePost
        {
            public virtual string message { get; set; }
            public virtual string from { get; set; }
            public virtual string to { get; set; }
        }
        public class NotificationPost
        {
            public virtual string Notification { get; set; }
            public virtual string from { get; set; }
            public virtual string to { get; set; }
            public virtual string img { get; set; }
        }
    }
}
