namespace DriverReports.WebApi.Contracts.FinancialOperation
{
    public class UpdateFinancialOperationRequest
    {
        public Guid? UserId { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public int Type { get; set; }

        public string? Comment { get; set; }
    }
}
