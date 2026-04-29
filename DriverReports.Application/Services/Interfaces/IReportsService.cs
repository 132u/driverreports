using DriverReports.Application.DTOs.Reports;
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
    }
}
