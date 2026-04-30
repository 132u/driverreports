using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Services.Interfaces;
using DriverReports.WebApi.Contracts.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var createReportDto = new CreateReportDto(request.UserId, request.Date, request.Price, request.Description, request.PaymentType);
            var id = await _reportsService.CreateReportAsync(createReportDto, token);
            return Ok(id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports(CancellationToken token)
        {
            var result = await _reportsService.GetAllReportsAsync(token);
            return Ok(result);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetReportsByUserId(Guid userId,CancellationToken token)
        {
            var result = await _reportsService.GetReportsByUserIdAsync(userId, token);
            return Ok(result);
        }
    }
}
