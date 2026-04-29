using DriverReports.WebApi.Contracts.Report;
using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.WebApi.Controllers
{
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
            var createRrportDto = new CreateReportDto(request.UserId, request.Date, request.Price, request.Description, request.PaymentType);
            var id = await _reportsService.CreateReportAsync(createRrportDto, token);
            return Ok(id);
        }
    }
}
