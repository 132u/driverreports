using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.Reports
{
    public record CreateReportDto(
        Guid? DriverId,
        DateTime ReportDate, 
        decimal Price, 
        MoneyHolder MoneyHolder, 
        string ClientName, 
        string Description, 
        PaymentType PaymentType, 
        List<string> ImagePaths);
}
