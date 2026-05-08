using DriverReports.Application.DTOs.FinancialOperation;
using DriverReports.Application.DTOs.FinancialOperations;

namespace DriverReports.Application.Services.FinancialOperation
{
    public interface IFinancialOperationService
    {
        Task<Guid> CreateAsync(
             CreateFinancialOperationDto request,
             Guid userId,
             CancellationToken cancellationToken);

        Task<IEnumerable<FinancialOperationDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<FinancialOperationDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    }
}
