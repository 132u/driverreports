using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Services.Interfaces;
using DriverReports.Domain.Entities;
using DriverReports.WebApi.Contracts.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DriverReports.WebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpPost]
        public async Task<ActionResult<CreateReportResponce>> Create(CreateReportRequest request, CancellationToken token)
        {
            if (!DateTime.TryParse(request.ReportDate, out var date))
                return BadRequest("Invalid date format");

            var createReportDto = new CreateReportDto(request.UserId, date, request.Price, request.moneyHolder, request.Description, request.PaymentType);
            var id = await _reportsService.CreateReportAsync(createReportDto, token);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetReports(CancellationToken token)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var roleValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleValue != null && int.Parse(roleValue) == (int)UserRole.Admin)
            {
                var result2 = await _reportsService.GetAllReportsAsync(token);
                return Ok(await _reportsService.GetAllReportsAsync(token));
            }
            
            var t = await _reportsService.GetReportsByUserIdAsync(Guid.Parse(userId), token);
            return Ok(await _reportsService.GetReportsByUserIdAsync(Guid.Parse(userId), token));
            //если админ,то все репорты, если водитель то только его репорты
            //var result = await _reportsService.GetAllReportsAsync(token);
           // return Ok(result);
        }   

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetReportsByUserId(Guid userId,CancellationToken token)
        {
            var result = await _reportsService.GetReportsByUserIdAsync(userId, token);
            return Ok(result);
        }
    }
}
