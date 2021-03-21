using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using DeliveryRoomWatcher.Parameters;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public  class EmailController : ControllerBase
    {
        [HttpPost]
        [Route("api/email/send")]
        public  void sendEmail(string toname,string to) {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("LANANG PREMIERE DOCTORS HOSPITAL", "edwardjosephfernandez@gmail.com"));
            message.To.Add(new MailboxAddress(toname,to));
            message.Subject = "Account Verified";

            message.Body = new TextPart("plain")
            {
                Text ="Your account is verified you can now login to your account"
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("edwardjosephfernandez@gmail.com", "iwillmarryGlory23");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
