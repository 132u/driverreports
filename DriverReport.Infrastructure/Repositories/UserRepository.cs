using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DriverReport.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> AddAsync(User user, CancellationToken token)
        {
            await _appDbContext.Users.AddAsync(user);
            return user.Id;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken token)
        {
            return await _appDbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken token)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<Guid> UpdateAsync(User user, CancellationToken token)
        {
            await _appDbContext.Users.Where(u => u.Id == user.Id).ExecuteUpdateAsync(u =>
            u.SetProperty(p => p.Email, user.Email)
            .SetProperty(p => p.Name, user.Name));
            //TODO: надо подумать как поменять роль
            //.SetProperty(p => p.Roles., user.Email));
            return user.Id;
        }
    }
}
