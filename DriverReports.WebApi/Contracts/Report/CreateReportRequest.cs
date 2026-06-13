using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.WebApi.Contracts.Report
{
    public record CreateReportRequest(
        Guid DriverId,
        DateTime ReportDate, 
        decimal Price, 
        MoneyHolder MoneyHolder, 
        string ClientName, 
        string Description, 
        PaymentType PaymentType, 
        List<string>? ImagePaths = null);
}
