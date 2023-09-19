using Microsoft.AspNetCore.Mvc;
using OrdersApi.Security.Authorization.Interfaces;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : SecureAccessController
    {
        public OrdersController(IAuthorizationProcessor authProcessor) : base(authProcessor)
        {
        }

        [HttpGet]
        [Route("TestAccess")]
        public IActionResult TestAccess()
        {
            return Ok("Success");
        }
    }
}
