using DriverReports.Domain.Entities;

namespace DriverReports.WebApi.Contracts.FinancialOperation
{
    public record CreateFinancialOperationRequest(
        DateTime Date,
        decimal Amount,
        FinancialOperationType Type,
        string? Comment);}
