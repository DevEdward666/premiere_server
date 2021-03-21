using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class DefaultController : ControllerBase
    {
        DefaultsRepo _default = new DefaultsRepo();

        [HttpPost]
        [Route("api/default/getregion")]
        public IActionResult getregion()
        {
            return Ok(_default.getRegion());
        }

        [HttpPost]
        [Route("api/default/getprovince")]
        public IActionResult getprovince(PDefaults def)
        {
            return Ok(_default.getProvince(def));
        }

        [HttpPost]
        [Route("api/default/getnationality")]
        public IActionResult getnationality()
        {
            return Ok(_default.getNationality());
        }

        [HttpPost]
        [Route("api/default/getcity")]
        public IActionResult getcity(PDefaults def)
        {
            return Ok(_default.getCity(def));
        }


        [HttpPost]
        [Route("api/default/getbarangay")]
        public IActionResult getbarangay(PDefaults def)
        {
            return Ok(_default.getBarangay(def));
        }
    }
}
