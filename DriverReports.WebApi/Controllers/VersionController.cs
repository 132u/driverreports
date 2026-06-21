using Microsoft.AspNetCore.Mvc;

namespace DriverReports.API.Controllers;

[ApiController]
[Route("api/version")]
public class VersionController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            version = "1.0.1",
            buildDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            commit = "driver id nullable fix"
        });
    }
}