using DriverReports.Domain.Entities;

namespace DriverReports.Application.Interfaces
{
    public interface IReportRepository
    {
        Task<Report?> GetByIdAsync(Guid id);
        Task<IEnumerable<Report>> GetAllAsync();
        Task<IEnumerable<Report>> GetByDriverIdAsync(Guid driverId);
        Task AddAsync(Report report);
        Task UpdateAsync(Report report);
        Task DeleteAsync(Guid id);
    }
}
