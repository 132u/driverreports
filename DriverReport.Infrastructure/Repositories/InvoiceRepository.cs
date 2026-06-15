using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DriverReports.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _appDbContext;
        public InvoiceRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> AddAsync(Invoice invoice, CancellationToken token)
        {
            var result = await _appDbContext.Invoices.AddAsync(invoice);
            return invoice.Id;
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync(int year, int month, CancellationToken token)
        {
            return await _appDbContext.Invoices.AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(Invoice invoice, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
