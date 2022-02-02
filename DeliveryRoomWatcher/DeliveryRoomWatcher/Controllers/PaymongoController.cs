using System.Threading.Tasks;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Mvc;
using static DeliveryRoomWatcher.Models.PaymongoModel;
using static DeliveryRoomWatcher.Payloads.PaymongoPayloads;

namespace DeliveryRoomWatcher.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymongoController : ControllerBase
    {
        PaymongoRepo paymongo_repo = new PaymongoRepo();

        [HttpPost]
        public async Task<IActionResult> EWalletCreateSourceAsync(PaymongoEwalletPayload payload)
        {
            return Ok(await paymongo_repo.EWalletCreateSourceAsync(payload));
        }


        [HttpPost]
        public async Task<IActionResult> EWalletWebHookAsync(PaymongoSourceResourceResponse payload)
        {
            var res = await paymongo_repo.EWalletWebHookAsync(payload);

            if (res.success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult EWalletPaidWebHook(PaymongoSourceResourceResponse payload)
        {

            var res = paymongo_repo.EWalletPaidWebHook(payload);

            if (res.success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }



        //CARD PAYMENT 

        [HttpPost]
        public async Task<IActionResult> CreatePaymentIntentAsync(PaymongoEwalletPayload payload)
        {
            return Ok(await paymongo_repo.CreatePaymentIntentAsync(payload));
        }
        [HttpPost]
        public async Task<IActionResult> PaidPaymentIntentAsync(PaymongoEwalletPayload payload)
        {
            return Ok(await paymongo_repo.PaidPaymentIntentAsync(payload));
        }
        //RECORDS
        [HttpPost]
        public IActionResult GetTablePaymongoLog(PaymongoTablePayload payload)
        {
            return Ok(paymongo_repo.GetTablePaymongoLog(payload));
        }
    }
}

