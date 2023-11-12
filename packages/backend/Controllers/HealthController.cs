using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace YaleAccess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Health()
        {
            Log.Logger.Information("Hit the health endpoint.");
            return Ok("Service is healthy");
        }        
    }
}