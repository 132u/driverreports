using DriverReports.Application.Services.FinancialSummary;
using DriverReports.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/driver-summaries")]
    public class DriverSummariesController : BaseController
    {
        private readonly ISummaryService _summaryService;

        public DriverSummariesController(
            ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        // =====================================================
        // 👤 МОИ ИТОГИ ПО МЕСЯЦАМ
        // =====================================================

        /// <summary>
        /// Получить итоговые финансовые отчеты текущего водителя.
        /// </summary>
        [HttpGet("my")]
        public async Task<IActionResult> GetMySummary(
            CancellationToken token)
        {
            var result = await _summaryService
                .GetDriverMonthlySummaryAsync(UserId, token);

            return Ok(result);
        }

        // =====================================================
        // 👤 МОИ ДЕТАЛИ ЗА МЕСЯЦ
        // =====================================================

        /// <summary>
        /// Получить детальную информацию текущего водителя за месяц.
        /// </summary>
        [HttpGet("my/details")]
        public async Task<IActionResult> GetMyMonthlyDetails(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var result = await _summaryService
                .GetDriverMonthlyDetailsAsync(
                    UserId,
                    year,
                    month,
                    token);
            var rows = result.Rows.Where(r => r.ClientName != null || r.Cash != null || r.NonCashWithoutVat != null || r.NonCashWithVat != null);
            return Ok(rows);
        }

        // =====================================================
        // 👑 АДМИН: ВСЕ ВОДИТЕЛИ
        // =====================================================

        /// <summary>
        /// Получить итоговые отчеты всех водителей за год.
        /// Только для администратора.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllDriversSummary(
            [FromQuery] int year,
            CancellationToken token)
        {
            var result = await _summaryService
                .GetAllDriversMonthlySummaryAsync(year, token);

            return Ok(result);
        }

        // =====================================================
        // 👑 АДМИН: КОНКРЕТНЫЙ ВОДИТЕЛЬ
        // =====================================================

        /// <summary>
        /// Получить итоговые финансовые отчеты конкретного водителя.
        /// Только для администратора.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetDriverSummary(
            Guid driverId,
            CancellationToken token)
        {
            var result = await _summaryService
                .GetDriverMonthlySummaryAsync(driverId, token);

            return Ok(result);
        }

        // =====================================================
        // 👑 АДМИН: ДЕТАЛИ ВОДИТЕЛЯ
        // =====================================================

        /// <summary>
        /// Получить детальную информацию водителя за месяц.
        /// Только для администратора.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{driverId}/details")]
        public async Task<IActionResult> GetDriverMonthlyDetails(
            Guid driverId,
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var result = await _summaryService
                .GetDriverMonthlyDetailsAsync(
                    driverId,
                    year,
                    month,
                    token);
            var rows = result.Rows.Where(r => r.ClientName != null || r.Cash!= null || r.NonCashWithoutVat!= null || r.NonCashWithVat != null);
            return Ok(rows);
        }
    }
}