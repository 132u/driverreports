using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.FinancialOperation
{
    public record CreateFinancialOperationDto(
        DateTime Date,
        decimal Amount,
        FinancialOperationType Type,
        string? Comment
    );
}
