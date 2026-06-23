using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.Reports
{
    public record UpdateReportDto(
        Guid? DriverId,
        DateTime ReportDate, 
        decimal Price, 
        MoneyHolder MoneyHolder, 
        string ClientName, 
        string Description, 
        PaymentType PaymentType, 
        List<string> ImagePaths);
}
