using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Controller
{
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        [Route("api/start")]
        public ActionResult start()
        {


            return Ok("The server has started successfully");
        }
    }
}
