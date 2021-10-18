using DeliveryRoomWatcher.Repositories.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace DeliveryRoomWatcher.Controllers.Employee
{
    [ApiController]
   
    public class MemoController : ControllerBase
    {
        MemoRepo _memo = new MemoRepo();


        [HttpPost]
        [Route("api/memo/getmemo")]
        public ActionResult getmemo()
        {
            return Ok(_memo.getmemo());
        }
    }
}
