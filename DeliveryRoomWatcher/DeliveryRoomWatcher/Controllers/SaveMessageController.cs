using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models.User;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class SaveMessageController : ControllerBase
    {
        MessagesRepo _message = new MessagesRepo();
        [HttpPost]
        [Route("api/messages/getmessages")]
        public ActionResult getmessages( SignalR_userMessageDetails.MessageDetail messageDetail)
        {
            return Ok(_message.getmessages(messageDetail));
        }   
        [HttpPost]
        [Route("api/messages/getusers")]
        public ActionResult getusers( SignalR_userMessageDetails.MessageUsers messageDetail)
        {
            return Ok(_message.getusers(messageDetail));
        }
        [HttpPost]
        [Route("api/messages/sendmessage")]
        public ActionResult sendmessage(SignalR_userMessageDetails.MessageDetail messageDetail)
        {
            return Ok(_message.sendmessage(messageDetail));
        }
    }
}
