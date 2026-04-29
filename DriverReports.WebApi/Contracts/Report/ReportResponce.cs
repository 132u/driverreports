using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.WebApi.Contracts.Report
{
    public record ReportResponce(Guid id, Guid userId, DateTime date, decimal price, string description, PaymentType paymentType);
}
