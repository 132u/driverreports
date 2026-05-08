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
            var dto = _mapper.Map<CreateReportDto>(request);
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
    }
}
