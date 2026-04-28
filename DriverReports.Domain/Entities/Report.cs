namespace DriverReports.Domain.Entities;

public enum PaymentType
{
    Cash,
    CashlessWithVAT,
    CashlessWithoutVAT
}

public class Report
{
    public Guid Id { get; private set; }
    public Guid DriverId { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public PaymentType PaymentType { get; private set; }

    public Report(Guid driverId, DateTime date, decimal price, string description, PaymentType paymentType)
    {
        Id = Guid.NewGuid();
        DriverId = driverId;
        Date = date;
        Price = price;
        Description = description;
        PaymentType = paymentType;
    }

    public void Update(DateTime date, decimal price, string description, PaymentType paymentType)
    {
        if (price < 0) throw new InvalidOperationException("Цена не может быть отрицательной");
        if (date > DateTime.Now) throw new InvalidOperationException("Дата не может быть в будущем");

        Date = date;
        Price = price;
        Description = description;
        PaymentType = paymentType;
    }

    public static (Report report, string Error) Create(Guid driverId, DateTime date, decimal price, string description, PaymentType paymentType)
    {
        var error = string.Empty;
        if (string.IsNullOrEmpty(description) || price <= 0 || date > DateTime.Now)
        {
            error = "Описание пустое или дата в будущем или цена невалидная";
        }

        var report = new Report(driverId, date, price, description, paymentType);
        return (report, error);
    }
}