using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.User
{
    public class SignalR_userModel
    {
        public class UserDetail
        {
            public string ConnectionId { get; set; }
            public string UserName { get; set; }
        }
    }
}
