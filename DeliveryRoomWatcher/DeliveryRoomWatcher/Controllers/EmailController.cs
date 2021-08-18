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
using DeliveryRoomWatcher.Repositories;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public  class EmailController : ControllerBase
    {
        CompanyRepository company = new CompanyRepository();
        [HttpPost]
        [Route("api/email/send")]
        public  void sendEmail(string toname,string to) {
            var message = new MimeMessage();
            string hospitalame = company.CompanyName().data.ToString();
            message.From.Add(new MailboxAddress(hospitalame, "tuosolutions@gmail.com"));
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
                client.Authenticate("tuosolutions@gmail.com", "Dominion_01");

                client.Send(message);
                client.Disconnect(true);
            }
        }
        [HttpPost]
        [Route("api/email/sendOTP")]
        public void sendOTP(string toname, string to,string otp)
        {
            var message = new MimeMessage();
            string hospitalame = company.CompanyName().data.ToString();
            message.From.Add(new MailboxAddress(hospitalame, "tuosolutions@gmail.com"));
            message.To.Add(new MailboxAddress(toname, to));
            message.Subject = "OTP";

            message.Body = new TextPart("plain")
            {
                Text = "This is your OTP " +otp+" it will expire in 300 seconds"
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);

           
                client.Authenticate("tuosolutions@gmail.com", "Dominion_01");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
