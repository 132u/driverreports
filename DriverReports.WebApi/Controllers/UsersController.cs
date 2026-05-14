using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.DTOs.Users;
using DriverReports.Application.Services.Users;
using DriverReports.Domain.Entities;
using DriverReports.WebApi.Contracts.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserDto userDto, CancellationToken token)
        {
            var createUser = new CreateUserDto(userDto.id, userDto.name, userDto.email, userDto.passwordHash, userDto.role);
            var id = await _userService.CreateUserAsync(createUser, token);
            return Ok(id);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(CancellationToken token)
        {
            var users = await _userService.GetUsersAsync(token);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("drivers")]
        public async Task<IActionResult> GetDrivers(CancellationToken token)
        {
            var drivers = await _userService.GetDriversAsync(token);

            return Ok(drivers);
        }
    }
}
