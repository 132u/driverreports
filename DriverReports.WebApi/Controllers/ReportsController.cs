using AutoMapper;
using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Services.Reports;
using DriverReports.Domain.Entities;
using DriverReports.WebApi.Contracts.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : BaseController
    {
        private readonly IReportsService _reportsService;
        private readonly IMapper _mapper;
        public ReportsController(
            IReportsService reportsService,
            IMapper mapper)
        {
            _reportsService = reportsService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateReportRequest request, CancellationToken token)
        {
            var dto = new CreateReportDto(
                request.DriverId,
                request.ReportDate,
                request.Price,
                request.MoneyHolder,
                request.ClientName,
                request.Description,
                request.PaymentType,
                request.ImagePaths
             );
            var id = await _reportsService.CreateAsync(dto, UserId, token);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            IEnumerable<Report> reports;
            // 🔥 ADMIN
            if (IsAdmin)
            {
                reports = await _reportsService.GetAllAsync(token);
            }
            else
            {
                // 🔥 DRIVER
                reports = await _reportsService.GetByUserIdAsync(UserId, token);
            }
            return Ok(reports);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetByDriverid(
                        Guid driverId,
                         [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            IEnumerable<Report> reports;
            
            reports = await _reportsService.GetMothlyByUserIdAsync(driverId,year, month, token);
            
            return Ok(reports);
        }



        [HttpGet("details/{reportId}")]
        public async Task<IActionResult> GetReportDetails(
            Guid reportId,
            CancellationToken token)
        {
            var result = await _reportsService.GetByReportIdAsync(reportId, token);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyMonthlyReports(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var reports = await _reportsService.GetDriverMonthlyReportsListAsync(UserId, year, month, token);
            return Ok(reports);
        }
    }
}
