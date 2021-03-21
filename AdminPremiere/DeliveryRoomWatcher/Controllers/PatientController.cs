using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdminPremiere.Parameters;
using AdminPremiere.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Drawing.ImageConverter;

namespace AdminPremiere.Controllers
{

    [ApiController]
    public class PatientController : ControllerBase
    {
        PatientRepo _patients = new PatientRepo();
        [HttpPost]
        [Route("api/users/getusers")]
        public IActionResult getuserstable()
        {
            return Ok(_patients.getUserstable());
        }
        [HttpPost]
        [Route("api/users/getusersinfo")]
        public IActionResult getUserInfo(PUsers.PGetUsersInfo prem)
        {
            return Ok(_patients.getUserInfo(prem));
        }
    
        [Route("api/user/getimage")]
        [HttpPost]
        public IActionResult Post(PUsers.PGetUsersImage prem)
        {

            //using (System.Net.WebClient webClient = new System.Net.WebClient())
            //{
            //    using (Stream stream = webClient.OpenRead("C:\\Users\\TUO-Edward\\source\\repos\\DeliveryRoomWatcher\\DeliveryRoomWatcher\\" + username))
            //    {
            //        return File(stream, "image/jpeg");
            //    }
            //}

            string path = Path.Combine("C:\\Users\\TUO-Edward\\source\\repos\\DeliveryRoomWatcher\\DeliveryRoomWatcher\\" + prem);
            var image = path;
            return PhysicalFile(image, "image/jpeg");


        }
    }
}
