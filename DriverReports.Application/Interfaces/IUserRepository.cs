using DriverReports.Domain.Entities;

namespace DriverReports.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken token);
        Task<User?> GetByEmailAsync(string email, CancellationToken token);
        Task<Guid> AddAsync(User user, CancellationToken token);
        Task<Guid> UpdateAsync(User user, CancellationToken token);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken token);
    }
}
