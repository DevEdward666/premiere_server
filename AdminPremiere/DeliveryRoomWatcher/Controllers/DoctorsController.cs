using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [Authorize]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        DoctorsRepo _doctors = new DoctorsRepo();
        [HttpPost]
        [Route("api/doctors/getdoctors")]
        public IActionResult getdoctors(PDoctors.PGetDoctors doctors)
        {
            return Ok(_doctors.getdoctors(doctors));
        }  
        [HttpPost]
        [Route("api/doctors/getdoctorsinfo")]
        public IActionResult getdoctorsinfo(PDoctors.PGetDoctorsInfo doccode)
        {
            return Ok(_doctors.getdoctorsinfo(doccode));
        }
        [HttpPost]
        [Route("api/doctors/getdoctorsbyspecialty")]
        public IActionResult getdoctorsbyspecialty(PDoctors.PGetDoctorsDetails doccode)
        {
            return Ok(_doctors.getdoctorsbyspecialty(doccode));
        }
        [HttpPost]
        [Route("api/doctors/getdoctorsspecialty")]
        public IActionResult getdoctorsspecialty()
        {
            return Ok(_doctors.getdoctorsspecialty());
        }

    }
}
