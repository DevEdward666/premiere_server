using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Hubs;
using DeliveryRoomWatcher.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryRoomWatcher.Controllers.SignalR
{
    [ApiController]
    public class MessageController : Controller
    {
        protected readonly IHubContext<MessageHub> _messageHub;
        public MessageController([NotNull]IHubContext<MessageHub> messagehub)
        {
            _messageHub = messagehub;
        }
        [HttpPost]
        [Route("api/message")]
        [HttpPost]
        public async Task<IActionResult> SendMessage(mdlMessagePost.MessagePost messagePost)
        {
            await _messageHub.Clients.All.SendAsync("sendToReact", messagePost);
            return Ok();
        }

    }
}
