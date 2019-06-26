
namespace MultiTenantApp.Website.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MultiTenantApp.Application.Interfaces;
    using System.Threading.Tasks;

    [AllowAnonymous]
    [Route("api/SendGridWebhooks")]
    public class SendGridController : Controller
    {
        private readonly ISendGridService _sendGridService;
        public SendGridController(ISendGridService sendGridService)
        {
            _sendGridService = sendGridService;
        }
        [HttpPost]
        [Route("InboundEmail")]
        public IActionResult ReceiveInboundEmail()
        {
            var result = _sendGridService.ParseInboundEmailWebhook(Request.Body);
            //more logic, what logic do you need? :-) 

            return Ok();
        }

        [HttpPost]
        [Route("Events")]
        public async Task<IActionResult> ReceiveEvents()
        {
            var result = await _sendGridService.ParseWebhookEventsAsync(Request.Body);
            //more logic, what logic do you need? :-) 

            return Ok();
        }
    }
}
