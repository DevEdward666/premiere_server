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
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Doctors;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using DeliveryRoomWatcher.Models.Common;
using System.Drawing;
using System.Drawing.Imaging;
using DeliveryRoomWatcher.Hooks;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public  class EmailController : ControllerBase
    {
        CompanyRepository company = new CompanyRepository();
        //[HttpPost]
        //[Route("api/email/sendWithAttachMents")]
        //public void sendEmail([FromForm] SendEmail sendEmail)
        //{
        //    string hospitalame = company.CompanyName().data.ToString();
        //    using (MailMessage message = new MailMessage(DefaultConfig._providerEmailAddress, sendEmail?.To))
        //    {

        //        //message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
        //        //message.To.Add(new MailboxAddress(sendEmail?.RecieversName, sendEmail?.To));
        //        message.Subject = sendEmail?.Subject;
        //        message.Body = sendEmail?.Body;
        //        if (sendEmail.attachments.Count>0)
        //        {
        //            foreach (var f in sendEmail.attachments)
        //            {
        //                string fileName = Path.GetFileName(f.FileName);
        //                message.Attachments.Add(new Attachment(f.OpenReadStream(), fileName));
        //            }
        //        }
        //        message.IsBodyHtml = false;
        //        using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient())
        //        {
                 
                  
        //            client.Host = "smtp.gmail.com";
        //            client.EnableSsl = true;
        //            NetworkCredential NetworkCred = new NetworkCredential(DefaultConfig._providerEmailAddress, DefaultConfig._providerEmailPass);
        //            // Note: only needed if the SMTP server requires authentication
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = NetworkCred;
        //            client.Port = 587;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            client.Send(message);
        //            client.Dispose();
        //        }
        //    }
        
        //}
        [HttpPost]
        [Route("api/email/sendWithAttachMents")]
        public ResponseModel sendEmailwithAttachments([FromForm] SendEmail sendEmail) {
         
            try
            {
                var message = new MimeMessage();
                string hospitalame = company.CompanyName().data.ToString();
                message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
                message.To.Add(new MailboxAddress(sendEmail?.RecieversName, sendEmail?.To));
                message.Subject = sendEmail?.Subject;

                message.Body = new TextPart("plain")
                {
                    Text = sendEmail?.Body
                };
                var builder = new BodyBuilder();
                byte[] fileBytes;
                var file = sendEmail.attachments;
                if (file.Count > 0)
                {
                    foreach (var f in file)
                    {
                        using (var ms = new MemoryStream())
                        {
                            f.CopyToAsync(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(f.FileName, fileBytes, MimeKit.ContentType.Parse(MediaTypeNames.Application.Pdf));
                    }
                }

                builder.HtmlBody = sendEmail.Body;
                message.Body = builder.ToMessageBody();
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate(DefaultConfig._providerEmailAddress, DefaultConfig._providerEmailPass);
                smtp.Send(message);
                smtp.Disconnect(true);
    
                return new ResponseModel
                {
                    success = true,
                    message = "Message Sent",
                };
               
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    success=false,
                    message = ex.Message,
                };
            }
         

        }      
        [HttpPost]
        [Route("api/email/send")]
        public  void sendEmail(string toname,string to) {
            var message = new MimeMessage();
            string hospitalame = company.CompanyName().data.ToString();
            message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
            message.To.Add(new MailboxAddress(toname,to));
            message.Subject = "Account Verified";

            message.Body = new TextPart("plain")
            {
                Text ="Your account is verified you can now login to your account"
            };
      
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(DefaultConfig._providerEmailAddress, DefaultConfig._providerEmailPass);

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
            message.From.Add(new MailboxAddress(hospitalame, DefaultConfig._providerEmailAddress));
            message.To.Add(new MailboxAddress(toname, to));
            message.Subject = "OTP";

            message.Body = new TextPart("plain")
            {
                Text = "This is your OTP " +otp+" it will expire in 300 seconds"
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587,false);

           
                client.Authenticate(DefaultConfig._providerEmailAddress, DefaultConfig._providerEmailPass);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
