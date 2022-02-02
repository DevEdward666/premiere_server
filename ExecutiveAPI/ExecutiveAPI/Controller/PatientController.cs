using ExecutiveAPI.Model.PatientsModel;
using ExecutiveAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Controller
{
    [Authorize]
    [ApiController]
    public class PatientController : ControllerBase
    {
        PatientsRepo patient = new PatientsRepo();
        [HttpPost]
        [Route("api/patient/gettotalpatientOptions")]
        public ActionResult gettotalpatientOptions(PatientModel.GetPatientOptions options)
        {

            return Ok(patient.gettotalpatientOptions(options));
        }
        [HttpPost]
        [Route("api/patient/getpatientcomparedfivedays")]
        public ActionResult getpatientcomparedfivedays()
        {

            return Ok(patient.getpatientcomparedfivedays());
        }

        [HttpPost]
        [Route("api/patient/gettotalpatientDefault")]
        public ActionResult gettotalpatientDefault()
        {

            return Ok(patient.gettotalpatientDefault());
        }
    }
}
