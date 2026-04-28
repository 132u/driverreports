using DriverReports.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services.Interfaces
{
    public interface IReportsService
    {
        Task<Guid> CreateReportAsync(
             CreateReportRequest request,
             CancellationToken cancellationToken);
    }
}
