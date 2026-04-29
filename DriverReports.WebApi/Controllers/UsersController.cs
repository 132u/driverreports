using DriverReports.WebApi.Contracts.Report;
using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DriverReports.Application.DTOs.Users;

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
        public async Task<ActionResult<CreateReportResponce>> Create(CreateUserDto userDto, CancellationToken token)
        {
            var createUser = new CreateUserDto(userDto.id, userDto.name, userDto.email, userDto.passwordHash, userDto.role);
            var id = await _userService.CreateUserAsync(createUser, token);
            return Ok(id);
        }
    }
}
