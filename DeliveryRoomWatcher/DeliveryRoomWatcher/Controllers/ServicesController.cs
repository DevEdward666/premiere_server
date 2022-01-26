using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Providers;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    //[Authorize]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        ServicesRepo _services = new ServicesRepo();
        [Route("api/services/getimage")]
        [HttpPost]
        public ActionResult Post(string imgname)
        {
            if (imgname == "undefined" || imgname == null)
            {
                imgname = "test";
                string path = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\services\pathology.jpg");
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");
            }
            else
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), imgname);
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");

            }
        }
        [HttpPost]
        [Route("api/services/getServices")]
        public ActionResult getServices(PServices.PGetServices services)
        {
            return Ok(_services.getServices(services));
        }     
        [HttpPost]
        [Route("api/services/InsertNewServices")]
        public ActionResult InsertNewServices([FromForm] ServicesModel.Create_Services create_services)
        {
            return Ok(_services.InsertNewServices(create_services));
        }
        [HttpPost]
        [Route("api/services/InsertNewServicesDesc")]
        public ActionResult InsertNewServicesDesc([FromForm] ServicesModel.Create_Services_desc create_services_desc)
        {
            return Ok(_services.InsertNewServicesDesc(create_services_desc));
        }
        [HttpPost]
        [Route("api/services/getservicesinfo")]
        public ActionResult getservicesinfo(PServices.GetServiceDescID services)
        {
            return Ok(_services.getservicesinfo(services));
        }   
        [HttpPost]
        [Route("api/services/UpdateNewServices")]
        public ActionResult UpdateNewServices([FromForm] ServicesModel.Create_Services update_services)
        {
            return Ok(_services.UpdateNewServices(update_services));
        }        [HttpPost]
        [Route("api/services/UpdateNewServicesWithoutImage")]
        public ActionResult UpdateNewServicesWithoutImage([FromForm] ServicesModel.Create_Services update_services)
        {
            return Ok(_services.UpdateNewServicesWithoutImage(update_services));
        }     
        [Route("api/services/UpdateServicesDesc")]
        public ActionResult UpdateServicesDesc([FromForm] ServicesModel.Create_Services_desc update_services)
        {
            return Ok(_services.UpdateServicesDesc(update_services));
        }   
        [Route("api/services/UpdateServicesDescWithoutImage")]
        public ActionResult UpdateServicesDescWithoutImage([FromForm] ServicesModel.Create_Services_desc update_services)
        {
            return Ok(_services.UpdateServicesDescWithoutImage(update_services));
        }  
        [Route("api/services/UpdateServicesInfo")]
        public ActionResult UpdateServicesInfo([FromForm] ServicesModel.Create_Services_info update_services)
        {
            return Ok(_services.UpdateServicesInfo(update_services));
        }   
        [Route("api/services/UpdateServicesInfoWithoutImage")]
        public ActionResult UpdateServicesInfoWithoutImage([FromForm] ServicesModel.Create_Services_info update_services)
        {
            return Ok(_services.UpdateServicesInfoWithoutImage(update_services));
        }
        [HttpPost]
        [Route("api/services/getservicesdesc")]
        public ActionResult getservicesdesc(PServices.GetServiceID services)
        {
            return Ok(_services.getservicesdesc(services));
        }    
    
    }
}
