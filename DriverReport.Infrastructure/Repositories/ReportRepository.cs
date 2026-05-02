using DriverReport.Infrastructure.Persistence;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReport.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _appDbContext;

        public ReportRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Report report)
        {
            await _appDbContext.Reports.AddAsync(report);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _appDbContext.Reports.Where(report => report.DriverId == id).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            //return await _appDbContext.Reports.AsNoTracking().ToListAsync();
            return await _appDbContext.Reports
                .AsNoTracking()
                .Select(r => new Report(
                    r.DriverId,
                    r.ReportDate,
                    r.DriverName,
                    r.Price,
                    r.MoneyHolder,
                    r.ClientName,
                    r.Description,
                    r.PaymentType,
                    null // 👈 вместо ImagePaths
                ))
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetByDriverIdAsync(Guid driverId)
        {
            return await _appDbContext.Reports.Where(report => report.DriverId == driverId).AsNoTracking().ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Reports.FirstOrDefaultAsync(report => report.DriverId == id);
        }

        public async Task UpdateAsync(Report report)
        {
            await _appDbContext.Reports.Where(r=>r.Id==report.Id)
                .ExecuteUpdateAsync(s =>
                   s.SetProperty(p=>p.DriverId , report.DriverId)
                    .SetProperty(p=>p.CreatedDate, report.CreatedDate)
                    .SetProperty(p=>p.UpdatedDate, report.UpdatedDate)
                    .SetProperty(p=>p.ReportDate, report.ReportDate)
                    .SetProperty(p => p.Price, report.Price)
                    .SetProperty(p => p.Description, report.Description)
                    .SetProperty(p => p.PaymentType, report.PaymentType));
        }
    }
}
