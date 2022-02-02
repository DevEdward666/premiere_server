using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    
    [ApiController]
    public class QueueController : ControllerBase
    {

        QueueRepo _queue = new QueueRepo();
        [HttpPost]
        [Route("api/queue/waiting")]
        public ActionResult waiting(Queue.waiting waitings)
        {
            return Ok(_queue.waiting(waitings));
        }
        [HttpPost]
        [Route("api/queue/generatenumberkiosk")]
        public ActionResult generatenumberkiosk(Queue.generatecounternumber cntr)
        {
            return Ok(_queue.generatenumberkiosk(cntr));
        }
        [HttpPost]
        [Route("api/queue/getuserqueuenumbers")]
        public ActionResult getuserqueuenumbers(Queue.getuserqueuenumber prem_id)
        {
            return Ok(_queue.getuserqueuenumbers(prem_id));
        }    
        [HttpPost]
        [Route("api/queue/getqueuemain")]
        public ActionResult getqueuemain()
        {
            return Ok(_queue.getqueuemain());
        }
        [HttpPost]
        [Route("api/queue/generatequeuenumber")]
        public ActionResult generatequeuenumber(Queue.generatenumber generatenumber)
        {
            return Ok(_queue.generatequeuenumber(generatenumber));
        }  
        [HttpPost]
        [Route("api/queue/getcounterlist")]
        public ActionResult getcounterlist(Queue.getlobbynos counterlist)
        {
            return Ok(_queue.getcounterlist(counterlist));
        }  
        
        [HttpPost]
        [Route("api/queue/getcountertype")]
        public ActionResult getcountertype()
        {
            return Ok(_queue.getcountertype());
        }   
        [HttpPost]
        [Route("api/queue/getcountermaintable")]
        public ActionResult getcountermaintable()
        {
            return Ok(_queue.getcountermaintable());
        }    
        [HttpPost]
        [Route("api/queue/getcounternumber")]
        public ActionResult getcounternumber(Queue.getcounterno counterno)
        {
            return Ok(_queue.getcounternumber(counterno));
        } 
        [HttpPost]
        [Route("api/queue/updateservedqueue")]
        public ActionResult updateservedqueue(Queue.UpdateQueue counterno)
        {
            return Ok(_queue.updateservedqueue(counterno));
        }   
        [HttpPost]
        [Route("api/queue/updatequeue")]
        public ActionResult updatequeue(Queue.UpdateQueue counterno)
        {
            return Ok(_queue.updatequeue(counterno));
        } 
        [HttpPost]
        [Route("api/queue/getqueuno")]
        public ActionResult getqueuno(Queue.getqueues getqueuno)
        {
            return Ok(_queue.getqueuno(getqueuno));
        }
        [HttpPost]
        [Route("api/queue/waitinglist")]
        public ActionResult waitinglist(Queue.waiting waitings)
        {
            return Ok(_queue.waitingList(waitings));
        }
    }
}
