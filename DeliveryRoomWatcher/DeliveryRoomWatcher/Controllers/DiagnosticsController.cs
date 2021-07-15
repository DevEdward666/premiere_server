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
    public class DiagnosticsController : ControllerBase
    {
        DiagnosticRepository _diagnostic = new DiagnosticRepository();
        [HttpPost]
        [Route("api/diagnostics/getAppointmentsRequestTable")]
        public ActionResult getAppointmentsRequestTable(PDIagnostics prem)
        {
            return Ok(_diagnostic.getAppointmentsRequestTable(prem));
        } 
        [HttpPost]
        [Route("api/diagnostics/getAppointmentsRequestTableFinished")]
        public ActionResult getAppointmentsRequestTableFinished(PDIagnostics prem)
        {
            return Ok(_diagnostic.getAppointmentsRequestTableFinished(prem));
        }  
        [HttpPost]
        [Route("api/diagnostics/getAppointmentsResultsList")]
        public ActionResult getAppointmentsResultsList(PDIagnostics prem)
        {
            return Ok(_diagnostic.getAppointmentsResultsList(prem));
        }
        [HttpPost]
        [Route("api/diagnostics/getLabReqPdf")]
        public IActionResult getLabReqPdf(PSingleString payload)
        {
            return Ok(_diagnostic.getLabReqPdf(payload.value));
        }
    }
}
