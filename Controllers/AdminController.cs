using Microsoft.AspNetCore.Mvc;
using OrdersApi.Authentication.Interfaces;
using OrdersApi.Authentication.Models;
using OrdersApi.Encryption;
using OrdersApi.Encryption.Interfaces;
using OrdersApi.Security.Authentication;
using System.Data.Common;
using System.Net;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        IAuthenticatorProcess _authenticator;

        public AdminController(IAuthenticatorProcess authenticator)
        {
            _authenticator = authenticator;
        }

        [HttpGet]
        [Route("CreateUser")]
        public IActionResult CreateUserAccount(string email, string userName, string password)
        {
            try
            {
                AccountCreationResult result = _authenticator.CreateNewAccount(email, userName, password);

                if (result.Success)
                {
                    return Ok("Account created successfully.");
                }
                else
                {
                    return base.StatusCode((int)HttpStatusCode.NotAcceptable, result.Message);
                }
            }
            catch (Exception ex)
            {
                return base.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
