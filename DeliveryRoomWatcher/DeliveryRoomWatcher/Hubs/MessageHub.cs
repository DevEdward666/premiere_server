using DeliveryRoomWatcher.Controllers.SignalR;
using DeliveryRoomWatcher.Models.User;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Hubs
{
    public class MessageHub : Hub
    {
        //static List<SignalR_userModel.UserDetail> ConnectedUsers = new List<SignalR_userModel.UserDetail>();
        //static List<SignalR_userMessageDetails.MessageDetail> CurrentMessage = new List<SignalR_userMessageDetails.MessageDetail>();
        //public override async Task OnConnectedAsync()
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        //    await base.OnConnectedAsync();
        //}
        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
        //    await base.OnDisconnectedAsync(exception);
        //}
        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.ReceiveMessage(user, message);
        //}

    }
}
