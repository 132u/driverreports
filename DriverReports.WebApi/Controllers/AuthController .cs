using DriverReports.Application.DTOs.Auth;
using DriverReports.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request, CancellationToken token)
        {
            var r = await _authService.Register(request, token);

            return Ok(r);
        }
    }
}
