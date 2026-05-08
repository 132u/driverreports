using DriverReports.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DriverReports.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Guid UserId =>
            Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        protected string Role => User.FindFirst(ClaimTypes.Role)?.Value;
        protected bool IsAdmin => User.IsInRole("Admin");
    }
}
