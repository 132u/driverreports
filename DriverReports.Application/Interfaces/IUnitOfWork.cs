using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
