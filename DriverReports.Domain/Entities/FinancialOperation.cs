namespace DriverReports.Domain.Entities
{
    public enum FinancialOperationType
    {
        Advance = 0,            // аванс водителю
        Settlement = 1,         // водитель отдает деньги Виктору
        BaseWorkPayment = 2,    // работа на базе
        FuelExpense = 3         // топливо (расход Виктора)
    }   

    public class FinancialOperation
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }
        public decimal Amount { get; set; }

        public FinancialOperationType Type { get; set; }

        public DateTime CreatedDate { get; private set; }
        public DateTime Date { get; set; }

        public string? Comment { get; set; }
        public FinancialOperation() { }
        private FinancialOperation(
            Guid userId,
            decimal amount,
            FinancialOperationType type,
            DateTime date,
            DateTime createdDate,
            string? comment
        )
        {
            UserId = userId;
            Amount = amount;
            Type = type;
            Date = date;
            CreatedDate = createdDate;
            Comment = comment;
        }

        public static (FinancialOperation? operation, string Error) Create
        (
            Guid userId,
            decimal amount,
            FinancialOperationType type,
            DateTime date,
            string? comment
        )
        {
            if (amount <= 0)
            {
                return (null, "Amount must be greater than 0");
            }

            if (date > DateTime.UtcNow)
            {
                return (null, "Date cannot be in the future");
            }

            var operation = new FinancialOperation(
                userId,
                amount,
                type,
                date.ToUniversalTime(),
                DateTime.UtcNow, // createdDate
                comment
            );

            return (operation, string.Empty);
        }
    }
}
