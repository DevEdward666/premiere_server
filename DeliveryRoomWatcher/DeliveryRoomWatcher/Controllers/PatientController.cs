using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class PatientController : ControllerBase
    {
        PatientRepo _patients = new PatientRepo();
        [HttpPost]
        [Route("api/users/getusers")]
        public ActionResult getuserstable()
        {
            return Ok(_patients.getUserstable());
        }
        [HttpPost]
        [Route("api/users/getusersforapproval")]
        public ActionResult getUserstableForApproval()
        {
            return Ok(_patients.getUserstableForApproval());
        }
        [HttpPost]
        [Route("api/users/getusersinfo")]
        public ActionResult getUserInfo(PUsers.PGetUsersInfo prem)
        {
            return Ok(_patients.getUserInfo(prem));
        }
        [HttpPost]
        [Route("api/users/approveuser")]
        public ActionResult ApproveUser(PUsers.PUpdateUserInfo prem)
        {

            return Ok(_patients.UpdateUser(prem));
        }
        [HttpPost]
        [Route("api/users/addDiagnosticAppointment")]
        public ActionResult addDiagnosticAppointment(PUsers.PAddDiagnosticAppointment prem)
        {

            return Ok(_patients.addDiagnosticAppointment(prem));
        }   
        [HttpPost]
        [Route("api/users/addDiagnosticAppointmentothers")]
        public ActionResult addDiagnosticAppointmentothers(PUsers.PAddDiagnosticAppointmentOthers prem)
        {

            return Ok(_patients.addDiagnosticAppointmentothers(prem));
        }     
           
        [HttpPost]
        [Route("api/users/addDiagnosticProcedure")]
        public ActionResult addDiagnosticProcedure(PUsers.PAddDiagnosticAppointmentProcedure prem)
        {

            return Ok(_patients.addDiagnosticProcedure(prem));
        }     

        [HttpPost]
        [Route("api/users/deactivate")]
        public ActionResult Deactivate(PUsers.PUpdateUserInfo prem)
        {

            return Ok(_patients.DeactivateUser(prem));
        }
    }
}
