using DriverReports.Application.Services.FinancialSummary;
using DriverReports.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.API.Controllers
{
    [Authorize]
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

        [HttpGet("cashless-vat")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCashlessVat(int month,  int year)
        {
            var total = await _service
                .GetCashlessWithVatTotalAsync(month, year);

            return Ok(new
            {
                total
            });
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
    [FromQuery] int year,
            [FromQuery] int month,
    CancellationToken token)
        {
            var result =
               await _service.GetSummaryAsync(
                   UserId,
                   year,
                   month,
                   token);

            return Ok(result);
        }
    }
}