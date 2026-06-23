using DriverReports.Application.DTOs.Reports;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Interfaces
{
    public interface IReportRepository
    {
        Task<Report?> GetByIdAsync(Guid id, CancellationToken token);
        Task<IEnumerable<Report>> GetAllAsync(CancellationToken token);
        Task<IEnumerable<Report>> GetByUserIdAsync(Guid driverId, CancellationToken token);
        Task<Guid> AddAsync(Report report, CancellationToken token);

        Task<decimal> GetCashlessWithVatTotalAsync(int month, int year, CancellationToken token);
        Task DeleteAsync(Guid id, CancellationToken token);
    }
}
