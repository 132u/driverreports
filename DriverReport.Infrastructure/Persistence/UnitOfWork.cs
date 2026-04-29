using DriverReports.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReport.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
