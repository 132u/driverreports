using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.FinancialOperations
{
    public record FinancialOperationDto(
        Guid Id,
        Guid UserId,
        decimal Amount,
        FinancialOperationType Type,
        DateTime Date,
        DateTime CreatedDate,
        string? Comment
    );
}