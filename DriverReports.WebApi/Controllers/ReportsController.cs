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

            var createReportDto = new CreateReportDto(request.UserId, date, request.Price, request.MoneyHolder, request.ClientName, request.Description, request.PaymentType, request.imagePath);
            var id = await _reportsService.CreateReportAsync(createReportDto, token);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetReports(CancellationToken token)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("UserId not found");

            var roleValue = User.FindFirst(ClaimTypes.Role)?.Value;

            // 🔥 ADMIN
            if (roleValue != null && int.Parse(roleValue) == (int)UserRole.Admin)
            {
                var allReports = await _reportsService.GetAllReportsAsync(token);

                var result = allReports.Select(r => new Report(
                    r.DriverId,
                    r.ReportDate,
                    r.DriverName,
                    r.Price,
                    r.MoneyHolder,
                    r.ClientName,
                    r.Description,
                    r.PaymentType,
                    r.ImagePath != null ? baseUrl + r.ImagePath : null
                ));

                return Ok(result);
            }

            // 🔥 DRIVER
            var reports = await _reportsService.GetReportsByUserIdAsync(
                Guid.Parse(userId),
                token
            );

            var result2 = reports.Select(r => new Report(
                r.DriverId,
                r.ReportDate,
                r.DriverName,
                r.Price,
                r.MoneyHolder,
                r.ClientName,
                r.Description,
                r.PaymentType,
                r.ImagePath != null ? baseUrl + r.ImagePath : null
            ));

            return Ok(result2);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetReportsByUserId(Guid userId,CancellationToken token)
        {
            var result = await _reportsService.GetReportsByUserIdAsync(userId, token);
            return Ok(result);
        }
    }
}
