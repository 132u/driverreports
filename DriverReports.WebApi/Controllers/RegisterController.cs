using DriverReports.Application.DTOs.Users;
using DriverReports.Application.Services.Interfaces;
using DriverReports.Domain.Entities;
using DriverReports.WebApi.Contracts.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class RegisterController
    {
        private readonly IUserService _userService;
        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(CreateUserDto createUserDto, CancellationToken token)
        //{
        //    var hash = BCrypt.Net.BCrypt.HashPassword(createUserDto.passwordHash);

        //    await _userService.CreateUserAsync(createUserDto, token);


        //    return Ok();
        //}
    }
}
