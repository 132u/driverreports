using DriverReports.Domain.Entities;

namespace DriverReports.WebApi.Contracts.FinancialOperation
{
    public record CreateFinancialOperationRequest(
         Guid? UserId,
        DateTime Date,
        decimal Amount,
        FinancialOperationType Type,
        string? Comment);}
