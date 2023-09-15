using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OrdersApi.Authentication.Interfaces;
using OrdersApi.Controllers.Models;
using OrdersApi.Data.Database.Interfaces;
using OrdersApi.Security.Authentication.Models;
using OrdersApi.Security.Authorization.Interfaces;
using System.Net;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        IAuthenticatorProcess _authenticator;
        IAuthorizationProcessor _authorizationProcessor;

        public LoginController(IAuthenticatorProcess authenticator, IAuthorizationProcessor authorizationProcessor)
        {
            _authenticator = authenticator;
            _authorizationProcessor = authorizationProcessor;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            try
            {
                // Validate inputs

                // Perform login
                AccountLoginResult result = _authenticator.PerformLogin(email, password);

                if (result.Success)
                {
                    LoginSuccessTokensContainer container = new LoginSuccessTokensContainer();
                    container.AccessToken = result.AccessToken;
                    container.RefreshToken = result.RefreshToken;
                    return Ok(container);
                }
                else
                {
                    return Unauthorized(result.Message);
                }

            }
            catch (Exception ex)
            {
                return base.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RefreshLogin")]
        public IActionResult RefreshLogin()
        {
            try
            {
                string refreshToken = "";

                if (Request.Headers.TryGetValue("Refresh-token", out StringValues refreshTokenValues))
                {
                    refreshToken = refreshTokenValues;
                }
                else
                {
                    return BadRequest("Refresh token missing.");
                }



            }
            catch (Exception ex)
            {
                return base.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
