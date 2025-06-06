using Microsoft.AspNetCore.Mvc;

namespace SportsCenter.Api.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("I am alive :)");
        }
    }
}