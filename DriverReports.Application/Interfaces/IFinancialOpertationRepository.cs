using DriverReports.Domain.Entities;

namespace DriverReports.Application.Interfaces
{
    public interface IFinancialOperationRepository
    {
        Task<IEnumerable<FinancialOperation>?> GetByUserIdAsync(Guid id, CancellationToken token);
        Task<FinancialOperation?> GetByIdAsync(Guid id, CancellationToken token);
        Task<IEnumerable<FinancialOperation>> GetAllAsync(CancellationToken token);
        Task<Guid> AddAsync(FinancialOperation operation, CancellationToken token);
        Task UpdateAsync(FinancialOperation operation, CancellationToken token);
    }
}
