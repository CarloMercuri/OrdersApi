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
                tokenData.TokenUuid = headerValue;
                if (tokenData is null)
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

                    _authProcessor.AdjustTokenRates(tokenData);
                    /*string s = @"select u.user_email, u.user_name, r.role_id, a.token_type, a.expiration_datetime
                                FROM access_tokens a 
                                inner join UserData u on a.user_id = u.user_id
                                inner join user_user_roles r on r.user_id = a.user_id
                                WHERE a.token_uuid = 'p0Xh+SSEcYkbd3GJIJYYSBuLMB88J50pq5JAUX0zGEqRynuImc3WRjlzhmX8x3PaT/Y03fTGcUgNoVFK+ZHW7tPBfyInI8OT2d2u19zHdTZqwm6O2lsL5+gu+l3wqOg1vPRwc23nivksUaZZ4cLo6wqkC8C7RvrIvG5YPlqAPZg='
                                "
                    */
                }
                else
                {
                    // If the period is over anwyay, adjust the rate period and allow to proceed
                    if(tokenData.RateLimits.PeriodEnd < DateTime.Now)
                    {
                        _authProcessor.AdjustTokenRates(tokenData);
                    }
                    else
                    {
                        // Otherwise, rate limit
                        context.Result = new UnauthorizedObjectResult("Rate Limit Reached.");
                        return; 
                    }
                }

            }
            else
            {
                context.Result = new UnauthorizedObjectResult("Missing Access Token.");
            }



                
        }
    }
}
