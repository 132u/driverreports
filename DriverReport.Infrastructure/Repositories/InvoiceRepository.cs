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
            return await _appDbContext
                .Invoices
                .Where(i => i.InvoiceDate.Year == year && i.InvoiceDate.Month == month)
                .AsNoTracking().ToListAsync();
        }

        public async Task<decimal> GetTotalInvoicesAmountAsync(int year, int month, CancellationToken token)
        {
            return await _appDbContext
                .Invoices
                .Where(i => i.InvoiceDate.Year == year && i.InvoiceDate.Month == month)
                .SumAsync(i => i.Amount);

            //    var query = _appDbContext.Invoices
            //.Where(i => i.InvoiceDate.Year == year &&
            //            i.InvoiceDate.Month == month);
            //    var s= query.ToQueryString();
            //    Console.WriteLine(query.ToQueryString());

            //    return await query.SumAsync(i => i.Amount, token);
        }

        public async Task UpdateAsync(Invoice invoice, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
