using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DriverReport.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _appDbContext;

        public ReportRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<Guid> AddAsync(Report report, CancellationToken token)
        {
            await _appDbContext.Reports.AddAsync(report, token);
            return report.Id;
        }

        public async Task<IEnumerable<Report>> GetAllAsync(CancellationToken token)
        {
            //return await _appDbContext.Reports.AsNoTracking().ToListAsync();
            return await _appDbContext.Reports
                .AsNoTracking()
                .Select(r => new Report(
                    r.DriverId,
                    r.ReportDate,
                    r.DriverName,
                    r.Price,
                    r.MoneyHolder,
                    r.ClientName,
                    r.Description,
                    r.PaymentType,
                    null // 👈 вместо ImagePaths
                ))
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetByUserIdAsync(Guid driverId, CancellationToken token)
        {
            return await _appDbContext.Reports.Where(report => report.DriverId == driverId).AsNoTracking().ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _appDbContext.Reports.FirstOrDefaultAsync(report => report.Id == id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var affectedRows = await _appDbContext.Reports.Where(r => r.Id == id).ExecuteDeleteAsync();
            if (affectedRows == 0)
            {
                throw new KeyNotFoundException(
                    $"Report {id} not found");
            }
        }

        public async Task<decimal> GetCashlessWithVatTotalAsync(int year, int month, CancellationToken token)
        {
            return await _appDbContext.Reports
                .Where(x =>
                    x.PaymentType == PaymentType.CashlessWithVAT &&
                    x.ReportDate.Month == month &&
                    x.ReportDate.Year == year)
                .SumAsync(x => x.Price);
        }
    }
}
