using DriverReports.Application.DTOs.Invoices;
using DriverReports.Application.Services.FinancialSummary;
using DriverReports.Application.Services.Invoice;
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
        private readonly IInvoiceService _invoiceService;

        public SummaryController(
            ISummaryService service,
            IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;    
            _service = service;
        }

        [HttpGet("cashless-vat")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCashlessVat(int year, int month, CancellationToken token)
        {
            var total = await _service
                .GetCashlessWithVatTotalAsync(year, month, token);

            return Ok(new
            {
                total
            });
        }

        [HttpGet("cashless-vat-balance")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCashlessVatBalance(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
           var result = await _service.GetTotalInvoicesAmountAsync(year, month, token);

            return Ok(result);
        }

        [HttpGet("invoices-summary")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetIvoicesSummary(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var cashlessVatAmount =
         await _service.GetCashlessWithVatTotalAsync(
             year,
             month,
             token);

            var invoicesAmount =
                await _service.GetTotalInvoicesAmountAsync(
                    year,
                    month,
                    token);

            var balance =
                _service.CalculateCashlessVATBalance(
                    cashlessVatAmount,
                    invoicesAmount,
                    token);

            return Ok(
                new InvoicesSummaryDto(
                    cashlessVatAmount,
                    invoicesAmount,
                    balance));
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