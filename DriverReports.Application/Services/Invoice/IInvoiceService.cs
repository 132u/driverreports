using DriverReports.Application.DTOs.FinancialOperation;
using DriverReports.Application.DTOs.FinancialOperations;
using DriverReports.Application.DTOs.Invoices;

namespace DriverReports.Application.Services.Invoice
{
    public interface IInvoiceService
    {
        Task<Guid> CreateAsync(
           CreateInvoiceDto request,
           CancellationToken cancellationToken);

        Task<IEnumerable<InvoiceDto>> GetAllAsync(int year, int month, CancellationToken cancellationToken);

    }
}
