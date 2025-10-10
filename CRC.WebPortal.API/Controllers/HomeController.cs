using Microsoft.AspNetCore.Mvc;

namespace CRC.WebPortal.API.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Root endpoint for API
    /// </summary>
    [HttpGet("")]
    public IActionResult Index()
    {
        return Ok(new 
        { 
            message = "CRC WebPortal API is running",
            version = "1.0.0",
            architecture = "Clean Architecture with CQRS",
            endpoints = new 
            {
                health = "/api/auth/health",
                signup = "POST /api/auth/signup"
            },
            timestamp = DateTime.UtcNow
        });
    }
}
