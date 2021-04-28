using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.User
{
    public class SignalR_userMessageDetails
    {
        public class MessageDetail
        {

            public string message { get; set; }
            public string receiver { get; set; }

            public string sender { get; set; }
            public int offset { get; set; }


        }
        public class MessageUsers
        {

            public string receiver { get; set; }


        }
    }
}
