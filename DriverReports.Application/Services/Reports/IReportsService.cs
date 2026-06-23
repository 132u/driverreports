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

        public Task<Report> GetByReportIdAsync(Guid reportId, CancellationToken cancellationToken);
        public Task<IEnumerable<Report>> GetDriverMonthlyReportsListAsync(Guid driverId, int year, int month, CancellationToken token);
        public Task<IEnumerable<Report>> GetCommonCashlessWithVATMothlyAsync(int year, int month, CancellationToken token);
        Task<IEnumerable<Report>> GetMothlyByUserIdAsync(Guid userId, int year, int month, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateReportDto dto, Guid id, CancellationToken token);
        Task DeleteAsync(Guid id, CancellationToken token);
    }
}
