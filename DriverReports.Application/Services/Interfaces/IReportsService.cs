using DriverReports.Application.DTOs.Reports;
using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services.Interfaces
{
    public interface IReportsService
    {
        Task<Guid> CreateReportAsync(
             CreateReportDto request,
             CancellationToken cancellationToken);

        Task<IEnumerable<Report>> GetAllReportsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<Report>>   GetReportsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
