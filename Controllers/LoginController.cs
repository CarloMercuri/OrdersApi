using Microsoft.AspNetCore.Mvc;
using OrdersApi.Data.Database.Interfaces;
using System.Net;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private IDatabaseConnection _connection;
        public LoginController(IDatabaseConnection _db)
        {
            _connection = _db;
        }

        [HttpGet]
        [Route("TestDb")]
        public IActionResult TestDb()
        {
            try
            {
                return _connection.TestConnection() == true ? Ok("Connection working") : Ok("Connection NOT working.");
            }
            catch (Exception ex)
            {
                return base.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
