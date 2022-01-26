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

        //public void SaveImage(setNewImage image)
        //{
        //    try
        //    {
        //        var base64img = $@"{image.base64}";
        //        string convert = base64img.Replace("data:image/png;base64,", String.Empty);
        //        var bytes = Convert.FromBase64String(convert);
        //        string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\News\", image.title + ".jpg");

        //        using (var imageFile = new FileStream(filePath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //            _manage.InsertNewNews(image);
        //        }
        //    }catch(Exception e)
        //    {
        //        Console.WriteLine(e.Message.ToString());
        //    }

        //}
        public ActionResult SaveImage([FromForm] setNewImage image)
        {
            return Ok(_manage.InsertNewNews(image));
        }
        [HttpPost]
        [Route("api/manage/getEvents")]
        public ActionResult getuserstable()
        {
            return Ok(_manage.getEvents());
        }  
        [HttpPost]
        [Route("api/manage/getEventsAdmin")]
        public ActionResult getEventsAdmin()
        {
            return Ok(_manage.getEventsAdmin());
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
        [Route("api/manage/getappdefaults")]
        public ActionResult getappdefaults()
        {
            return Ok(_manage.getappdefaults());
        }  
        [HttpPost]
        [Route("api/manage/getEventsthisday")]
        public ActionResult getEventsthisday()
        {
            return Ok(_manage.getEventsthisday());
        }
        [HttpPost]
        [Route("api/manage/getFiles")]
        public ActionResult getFiles(Models.FileModel.Search_File search_File)
        {
            return Ok(_manage.getFiles(search_File));
        }  
        [HttpPost]
        [Route("api/manage/UpdateTestimonials")]
        public ActionResult UpdateTestimonials(Models.TestimonialsModel.update_testimonials update_testimonials)
        {
            return Ok(_manage.UpdateTestimonials(update_testimonials));
        }   
        [HttpPost]
        [Route("api/manage/InsertNewTestimonials")]
        public ActionResult InsertNewTestimonials(Models.TestimonialsModel.create_testimonials create_Testimonials)
        {
            return Ok(_manage.InsertNewTestimonials(create_Testimonials));
        }      
        [HttpPost]
        [Route("api/manage/InsertNewFile")]
        public ActionResult InsertNewFile(Models.FileModel.Create_New_File create_file)
        {
            return Ok(_manage.InsertNewFile(create_file));
        } 
        [HttpPost]
        [Route("api/manage/UpdateFile")]
        public ActionResult UpdateFile(Models.FileModel.Create_New_File create_file)
        {
            return Ok(_manage.UpdateFile(create_file));
        }
        //[HttpPost]
        //[Route("api/manage/insertevents")]
        //public ActionResult insertevents(PEvents.PEvent events)
        //{
        //    var base64img = $@"{events.base64}";
        //    string convert = base64img.Replace("data:image/jpeg;base64,", String.Empty);
        //    var bytes = Convert.FromBase64String(convert);
        //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Events\", events.evtitle + ".jpg");

        //    using (var imageFile = new FileStream(filePath, FileMode.Create))
        //    {
        //        imageFile.Write(bytes, 0, bytes.Length);
        //        imageFile.Flush();
        //        return Ok(_manage.InsertNewEvents(events));
        //    }
        //}
        [HttpPost]
        [Route("api/manage/insertevents")]
        public ActionResult insertevents(PEvents.PEvent events)
        {
            var base64img = $@"{events.base64}";
            string convert = base64img.Replace("data:image/jpeg;base64,", String.Empty);
            var bytes = Convert.FromBase64String(convert);
            string unique_file_name ="_"
                          + Guid.NewGuid().ToString().Substring(0, 4) ;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Events\", unique_file_name + ".jpg");
            events.evimage = $@"Resources\Events\{unique_file_name}";
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
                return Ok(_manage.InsertNewEvents(events));
            }
        }
        [HttpPost]
        [Route("api/manage/InsertMasterFiles")]
        public ActionResult InsertMasterFiles(Models.FileModel files)
        {
            return Ok(_manage.InsertMasterFiles(files));
        }     
        [HttpPost]
        [Route("api/manage/InsertFiles")]
        public ActionResult InsertFiles(Models.FileModel files)
        {
            return Ok(_manage.InsertFiles(files));
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
