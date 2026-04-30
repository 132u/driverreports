namespace DriverReports.Domain.Entities;

public enum PaymentType
{
    Cash,
    CashlessWithVAT,
    CashlessWithoutVAT
}

public enum MoneyHolder
{
    Driver = 1,
    Victor = 2
}

public class Report
{
    public Guid Id { get; private set; }
    public Guid DriverId { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    public DateTime ReportDate { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public PaymentType PaymentType { get; private set; }

    public MoneyHolder MoneyHolder { get; set; }

    public Report(Guid driverId, DateTime reportDate, decimal price, MoneyHolder moneyHolder, string description, PaymentType paymentType)
    {
        Id = Guid.NewGuid();
        DriverId = driverId;
        CreatedDate = DateTime.Now.ToUniversalTime();
        UpdatedDate = DateTime.Now.ToUniversalTime();
        ReportDate = reportDate.ToUniversalTime();
        Price = price;
        MoneyHolder = moneyHolder;
        Description = description;
        PaymentType = paymentType;
    }

    public void Update(DateTime reportDate, decimal price, MoneyHolder moneyHolder, string description, PaymentType paymentType)
    {
        if (price < 0) throw new InvalidOperationException("Цена не может быть отрицательной");
        if (reportDate> DateTime.Now) throw new InvalidOperationException("Дата не может быть в будущем");
        
        ReportDate = reportDate.ToUniversalTime();
        UpdatedDate = DateTime.Now.ToUniversalTime();
        Price = price;
        Description = description;
        PaymentType = paymentType;
        MoneyHolder = moneyHolder;
    }

    public static (Report report, string Error) Create(Guid driverId, DateTime reportDate, decimal price, MoneyHolder moneyHolder, string description, PaymentType paymentType)
    {
        var error = string.Empty;
        if (string.IsNullOrEmpty(description) || price <= 0 || reportDate> DateTime.Now)
        {
            error = "Описание пустое или дата в будущем или цена невалидная";
        }

        var report = new Report(driverId, reportDate, price, moneyHolder, description, paymentType);
        return (report, error);
    }
}