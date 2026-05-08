using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.DTOs.Users;
using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services.Users
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(
             CreateUserDto request,
             CancellationToken cancellationToken);

        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
    }
}
