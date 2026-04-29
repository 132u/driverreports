using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.WebApi.Contracts.Report
{
    public record CreateReportRequest(Guid UserId, DateTime Date, decimal Price, string Description, PaymentType PaymentType);
}
