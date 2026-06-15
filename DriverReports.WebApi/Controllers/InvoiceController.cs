using DriverReports.Application.DTOs.FinancialOperation;
using DriverReports.Application.DTOs.Invoices;
using DriverReports.Application.Services.FinancialSummary;
using DriverReports.Application.Services.Invoice;
using DriverReports.Domain.Entities;
using DriverReports.WebApi.Contracts.FinancialOperation;
using DriverReports.WebApi.Contracts.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverReports.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController: BaseController
    {
        private readonly IInvoiceService _service;

        public InvoiceController(
            IInvoiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var result =
                await _service.GetAllAsync(
                    year,
                    month,
                    token);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceRequest request, CancellationToken token)
        {
            var invoiceDto = new CreateInvoiceDto(request.Amount, request.InvoiceDate, request.Comment);

            var id = await _service.CreateAsync(invoiceDto, token);

            return Ok(id);
        }
    }
}
