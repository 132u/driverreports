namespace DriverReports.Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime InvoiceDate { get; set; }
        public DateTime CreatedDate { get; private set; }
        public string? Comment { get; set; }

        public Invoice(
           decimal amount,
           DateTime invoiceDate,
           DateTime createdDate,
           string? comment
       )
        {
            Amount = amount;
            InvoiceDate = invoiceDate;
            CreatedDate = createdDate;
            Comment = comment;
        }

        public static (Invoice? operation, string Error) Create
        (
            decimal amount,
            DateTime invoiceDate,
            string? comment
        )
        {
            if (amount <= 0)
            {
                return (null, "Сумма должна быть больше 0");
            }

            if (invoiceDate > DateTime.UtcNow)
            {
                return (null, "Дата не может быть в будущем");
            }

            var invoice = new Invoice(
                amount,
                invoiceDate.ToUniversalTime(),
                DateTime.UtcNow, // createdDate
                comment
            );

            return (invoice, string.Empty);
        }

    }
}
