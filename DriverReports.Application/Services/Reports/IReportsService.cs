using DriverReports.Application.DTOs.Reports;
using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services.Reports
{
    public interface IReportsService
    {
        public Task<Guid> CreateAsync(
             CreateReportDto request,
             Guid userId,
             CancellationToken cancellationToken);

        public Task<IEnumerable<Report>> GetAllAsync(CancellationToken cancellationToken);

        public Task<IEnumerable<Report>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
