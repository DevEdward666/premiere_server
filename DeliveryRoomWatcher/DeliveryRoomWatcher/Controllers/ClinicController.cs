using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models.Clinic;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class ClinicController : ControllerBase
    {
        ClinicRepo _clinic = new ClinicRepo();
        [HttpPost]
        [Route("api/clinic/consultationDtls")]
        public ActionResult consultationDtls(consultation_table clinicmodels)
        {
            return Ok(_clinic.geConsultRequestDtls(clinicmodels));
        }    
        [HttpPost]
        [Route("api/clinic/consultationTable")]
        public ActionResult consultationTable(consultation_table clinicmodels)
        {
            return Ok(_clinic.geConsultRequestTable(clinicmodels));
        }
    }
}
