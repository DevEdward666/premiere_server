using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Providers;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [Authorize]
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
    }
}
