using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using OrdersApi.Security.Authorization.Interfaces;
using OrdersApi.Security.Authorization.Models;

namespace OrdersApi.Controllers
{
    [ApiController]    
    public class SecureAccessController : Controller
    {
        IAuthorizationProcessor _authProcessor;

        public SecureAccessController(IAuthorizationProcessor authProcessor)
        {
            _authProcessor = authProcessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var header = context.HttpContext.Request.Headers;
            if (header.TryGetValue("Access-Token", out StringValues accessTokenValues))
            {
                string headerValue = accessTokenValues;
                TokenData tokenData = _authProcessor.GetTokenData(headerValue);

                if(tokenData is null)
                {
                    context.Result = new UnauthorizedObjectResult("Token not found.");
                    return;
                }

                if(tokenData.Expiration > DateTime.Now)
                {
                    context.Result = new UnauthorizedObjectResult("Token expired.");
                    return;
                }

                if(tokenData.RateLimits.RequestsRemaining > 0)
                {

                }

            }
            else
            {
                context.Result = new UnauthorizedObjectResult("Missing Access Token.");
            }



                
        }
    }
}
