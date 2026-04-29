using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(
             CreateUserDto request,
             CancellationToken cancellationToken);
    }
}
