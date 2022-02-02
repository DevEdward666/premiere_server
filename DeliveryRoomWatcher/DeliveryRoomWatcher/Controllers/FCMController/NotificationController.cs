using DeliveryRoomWatcher.Models.FCM;
using DeliveryRoomWatcher.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Controllers.FCMController
{
   
        [Route("api/notification")]
        [ApiController]

        public class NotificationController : ControllerBase
        {
            private readonly INotificationService _notificationService;
            public NotificationController(INotificationService notificationService)
            {
                _notificationService = notificationService;
            }

        [Route("send2")]
        [HttpPost]
        public IActionResult SendNotification2Async(NotificationModel notificationModel)
        {
            var result = _notificationService.SendMessage(notificationModel);
            return Ok(result);
        }
        [Route("sendnotifitodoctors")]
            [HttpPost]
            public async Task<IActionResult> SendNotificationDoctors([FromForm] NotificationModel notificationModel)
            {
                return Ok(await _notificationService.SendNotificationDoctors(notificationModel));
            }   
        [Route("sendnotiftoemployee")]
            [HttpPost]
            public async Task<IActionResult> SendNotificationEmployee([FromForm] NotificationModel notificationModel)
            {
                return Ok(await _notificationService.SendNotificationEmployee(notificationModel));
            }        
        [Route("sendnotiftopremiereclient")]
            [HttpPost]
            public async Task<IActionResult> SendNotificationToPremiereClient([FromForm] NotificationModel notificationModel)
            {
                return Ok(await _notificationService.SendNotificationToPremiereClient(notificationModel));
            }
                     
        [Route("sendnotiftoworkbench")]
            [HttpPost]
            public async Task<IActionResult> SendNotificationToPremiereWorkBench([FromForm] NotificationModel notificationModel)
            {
                return Ok(await _notificationService.SendNotificationToPremiereWorkBench(notificationModel));
            }

 
      
    }
   
}
