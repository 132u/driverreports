using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.Reports
{
    public record CreateReportDto(
        DateTime ReportDate, 
        decimal Price, 
        MoneyHolder MoneyHolder, 
        string ClientName, 
        string Description, 
        PaymentType PaymentType, 
        List<string> ImagePaths);
}
