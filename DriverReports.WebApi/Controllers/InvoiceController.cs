using DriverReports.Application.DTOs.Invoices;
using DriverReports.Application.Services.FinancialOperation;
using DriverReports.Application.Services.FinancialSummary;
using DriverReports.Application.Services.Invoice;
using DriverReports.Application.Services.Reports;
using DriverReports.WebApi.Contracts.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController: BaseController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IReportsService _reportService;
        private readonly ISummaryService _summaryService;

        public InvoicesController(
            IInvoiceService service,
            IReportsService reportService,
            ISummaryService summaryService,
            IFinancialOperationService financialOperationService)
        {
            _invoiceService = service;
            _reportService = reportService;
            _summaryService = summaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var result =
                await _invoiceService.GetAllAsync(
                    year,
                    month,
                    token);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceRequest request, CancellationToken token)
        {
            var invoiceDto = new CreateInvoiceDto(request.Amount, request.InvoiceDate, request.Comment);

            var id = await _invoiceService.CreateAsync(invoiceDto, token);

            return Ok(id);
        }
    }
}
