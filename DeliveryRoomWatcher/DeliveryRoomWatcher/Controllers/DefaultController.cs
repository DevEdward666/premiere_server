using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models;
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
        public ActionResult getregion()
        {
            return Ok(_default.getRegion());
        }

        [HttpPost]
        [Route("api/default/getprovince")]
        public ActionResult getprovince(PDefaults def)
        {
            return Ok(_default.getProvince(def));
        }

        [HttpPost]
        [Route("api/default/getnationality")]
        public ActionResult getnationality()
        {
            return Ok(_default.getNationality());
        }

        [HttpPost]
        [Route("api/default/getcity")]
        public ActionResult getcity(PDefaults def)
        {
            return Ok(_default.getCity(def));
        }


        [HttpPost]
        [Route("api/default/getbarangay")]
        public ActionResult getbarangay(PDefaults def)
        {
            return Ok(_default.getBarangay(def));
        }
        [HttpPost]
        [Route("api/default/getcivilstatus")]
        public ActionResult getCivilStatus()
        {
            return Ok(_default.getCivilStatus());
        }
        [HttpPost]
        [Route("api/default/getreligion")]
        public ActionResult getReligion()
        {
            return Ok(_default.getReligion());
        }      
        [HttpPost]
        [Route("api/default/getProcedures")]
        public ActionResult getProcedures()
        {
            return Ok(_default.getProcedures());
        } 
        [HttpPost]
        [Route("api/default/insertNotifications")]
        public ActionResult insertNotifications(mdlNotifications.createnotifications notifications)
        {
            return Ok(_default.insertNotifications(notifications));
        } 
        [HttpPost]
        [Route("api/default/getnotications")]
        public ActionResult getnotications(mdlNotifications notifications)
        {
            return Ok(_default.getnotications(notifications));
        }  
        [HttpPost]
        [Route("api/default/getnoticationsAdmin")]
        public ActionResult getnoticationsAdmin(mdlNotifications.searchNotif notifications)
        {
            return Ok(_default.getnoticationsAdmin(notifications));
        }
    }
}
