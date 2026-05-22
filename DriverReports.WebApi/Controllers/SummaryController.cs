using DriverReports.Application.Services.FinancialSummary;
using DriverReports.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController : BaseController
    {
        private readonly ISummaryService _service;

        public SummaryController(
            ISummaryService service)
        {
            _service = service;
        }

        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetSummary(
            Guid driverId,
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var result =
                await _service.GetSummaryAsync(
                    driverId,
                    year,
                    month,
                    token);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMySummary(
    int year,
    int month,
    CancellationToken token)
        {
            return Ok();
            //var userId = User.GetUserId();

            //var result = await _service
            //    .GetMonthlySummaryAsync(
            //        UserId,
            //        year,
            //        month,
            //        token);

            //return Ok(result);
        }
    }
}