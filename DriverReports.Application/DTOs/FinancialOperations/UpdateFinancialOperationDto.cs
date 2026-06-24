namespace DriverReports.Application.DTOs.FinancialOperations
{
    public record UpdateFinancialOperationDto(Guid? UserId, DateTime Date, decimal Amount, string? Comment);
}
