using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.DTOs.Reports
{
    public record ReportDto(
        DateTime ReportDate,
        decimal Price,
        MoneyHolder MoneyHolder,
        string ClientName,
        string Description,
        PaymentType PaymentType,
        List<string> ImagePaths);
}
