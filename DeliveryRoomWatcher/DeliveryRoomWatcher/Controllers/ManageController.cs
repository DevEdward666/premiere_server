using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]

    public class ManageController : ControllerBase
    {
        ManageRepo _manage = new ManageRepo();

        [HttpPost]
        [Route("api/upload/UploadNewsImage")]
      
        public void SaveImage(setNewImage image)
        {
            var base64img =$@"{image.base64}";
                var bytes = Convert.FromBase64String(base64img);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\News\\", image.title + ".jpg");

            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
                _manage.InsertNewNews(image);
            }
         
        }
        [HttpPost]
        [Route("api/manage/getEvents")]
        public ActionResult getuserstable(PEvents.searchableDate date)
        {
            return Ok(_manage.getEvents(date));
        }
        [HttpPost]
        [Route("api/manage/getEventsInfo")]
        public ActionResult getevents(PEvents.PGetEvent events)
        {
            return Ok(_manage.getEventInfo(events));
        }
        [HttpPost]
        [Route("api/manage/updateevents")]
        public ActionResult updateevents(PEvents.PEvent events)
        {
            return Ok(_manage.UpdateEvents(events));
        } 
        [HttpPost]
        [Route("api/manage/getEventsBymonth")]
        public ActionResult getEventsBymonth(PEvents.PEventByMonth events)
        {
            return Ok(_manage.getEventsBymonth(events));
        } 
        [HttpPost]
        [Route("api/manage/getEventsthisweek")]
        public ActionResult getEventsthisweek()
        {
            return Ok(_manage.getEventsthisweek());
        }  
        [HttpPost]
        [Route("api/manage/getEventsthisday")]
        public ActionResult getEventsthisday()
        {
            return Ok(_manage.getEventsthisday());
        }
        [HttpPost]
        [Route("api/manage/insertevents")]
        public void insertevents(PEvents.PEvent events)
        {
            var base64img = $@"{events.base64}";
            var bytes = Convert.FromBase64String(base64img);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Events\\", events.evtitle + ".jpg");

            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
               _manage.InsertNewEvents(events);
            }
        }

        //[HttpPost]
        //[Route("api/user/UploadNewsImage")]
        //public ActionResult Post([FromForm] FileModel file)
        //{
        //    try
        //    {

        //        string path = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //            string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}\\", file.FileName);
        //            using (Stream stream = new FileStream(filepath, FileMode.Create))
        //            {
        //                file.FormFile.CopyTo(stream);
        //            }
        //        }
        //        else
        //        {
        //            string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}\\", file.FileName);
        //            using (Stream stream = new FileStream(filepath, FileMode.Create))
        //            {
        //                file.FormFile.CopyTo(stream);
        //            }

        //        }

        //        return StatusCode(StatusCodes.Status201Created);



        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}
