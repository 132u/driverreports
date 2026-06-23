using DriverReports.Domain.Entities;

namespace DriverReports.WebApi.Contracts.Report
{
    public record UpdateReportRequest(
        Guid? DriverId,
        DateTime ReportDate,
        decimal Price,
        string Description,
        string ClientName,
        PaymentType PaymentType,
        MoneyHolder MoneyHolder,
        List<string>? ImagePaths = null);
}
