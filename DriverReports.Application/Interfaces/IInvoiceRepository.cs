using DriverReports.Domain.Entities;

namespace DriverReports.Application.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetAllAsync(int year, int month, CancellationToken token);
        Task<Guid> AddAsync(Invoice invoice, CancellationToken token);
    }
}