using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.DTOs.Reports
{
    public record CreateReportDto(Guid UserId, DateTime Date, decimal Price, string Description, PaymentType PaymentType);
}
