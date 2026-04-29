using DriverReport.Infrastructure.Persistence;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReport.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(User user)
        {
            await _appDbContext.Users.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _appDbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            await _appDbContext.Users.Where(u => u.Id == user.Id).ExecuteUpdateAsync(u =>
            u.SetProperty(p => p.Email, user.Email)
            .SetProperty(p => p.Name, user.Name));
            //TODO: надо подумать как поменять роль
            //.SetProperty(p => p.Roles., user.Email));
        }
    }
}
