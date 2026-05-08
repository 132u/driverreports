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

        public async Task UpdateAsync(FinancialOperation operation, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
