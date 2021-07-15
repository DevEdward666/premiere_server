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
    public class NotificationController : ControllerBase
    {
        protected readonly IHubContext<NotifyHub> _notifyhub;
        public NotificationController([NotNull] IHubContext<NotifyHub> notifyhub)
        {
            _notifyhub = notifyhub;
        }
        [HttpPost]
        [Route("api/notification")]
        public async Task<IActionResult> SendMessage(mdlMessagePost.NotificationPost notificationPost)
        {
            await _notifyhub.Clients.All.SendAsync("notifytoreact", notificationPost);
            return Ok();
        }
    }
}
