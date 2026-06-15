namespace DriverReports.WebApi.Contracts.Invoice
{
    public record CreateInvoiceRequest(
        decimal Amount,
        DateTime InvoiceDate,
        string? Comment
    );
}
