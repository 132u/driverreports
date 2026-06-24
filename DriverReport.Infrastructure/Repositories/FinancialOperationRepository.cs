using DriverReports.Application.DTOs.FinancialOperations;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DriverReport.Infrastructure.Repositories
{
    public class FinancialOperationRepository : IFinancialOperationRepository
    {
        private readonly AppDbContext _appDbContext;
        public FinancialOperationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> AddAsync(FinancialOperation operation, CancellationToken token)
        {
            var result = await _appDbContext.FinancialOperations.AddAsync(operation);
            return operation.Id;
        }

        public async Task<IEnumerable<FinancialOperation>> GetAllAsync(CancellationToken token)
        {
            return await _appDbContext.FinancialOperations.AsNoTracking().ToListAsync();
        }

        public async Task<FinancialOperation?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _appDbContext.FinancialOperations.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FinancialOperation>> GetByUserIdAsync(Guid id, CancellationToken token)
        {
            return await _appDbContext.FinancialOperations.AsNoTracking().Where(f => f.UserId == id).ToListAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateFinancialOperationDto request, CancellationToken token)
        {
        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var affectedRows = await _appDbContext.FinancialOperations.Where(r => r.Id == id).ExecuteDeleteAsync();
            if (affectedRows == 0)
            {
                throw new KeyNotFoundException(
                    $"Financial Operation {id} not found");
            }
        }
    }
}
