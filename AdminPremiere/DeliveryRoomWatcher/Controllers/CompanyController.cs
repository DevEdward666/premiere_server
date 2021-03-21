using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        CompanyRepository _company = new CompanyRepository();

        [HttpPost]
        [Route("api/company/company-name")]
        public IActionResult companyName()
        {
            return Ok(_company.CompanyName());
        }

        [HttpPost]
        [Route("api/company/company-logo")]
        public IActionResult companyLogo()
        {
            return Ok(_company.CompanyLogo());
        }

        [HttpPost]
        [Route("api/company/company-tagline")]
        public IActionResult companyTagline()
        {
            return Ok(_company.CompanyTagLine());
        }

    }
}
