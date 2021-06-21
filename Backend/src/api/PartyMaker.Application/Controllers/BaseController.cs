using PartyMaker.Dto.WebApp;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PartyMaker.Application.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult GetActionResult(WebAppResponseDto response)
        {
            var statusCode = HttpStatusCode.OK;
            if (response.IsTimeout)
            {
                statusCode = HttpStatusCode.GatewayTimeout;
            }
            else if (response.IsInvalid)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (response.IsNotFound)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (!response.IsSuccess)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)statusCode, response);
        }
    }
}
