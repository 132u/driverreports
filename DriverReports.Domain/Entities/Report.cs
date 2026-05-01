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

    public string DriverName { get; private set; }
    public DateTime ReportDate { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public PaymentType PaymentType { get; private set; }

    public MoneyHolder MoneyHolder { get; set; }

    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    
    public string? ImagePath { get; set; }

    public string ClientName { get; private set; }

    public Report(
        Guid driverId, 
        DateTime reportDate, 
        string driverName, 
        decimal price, 
        MoneyHolder moneyHolder, 
        string clientName, 
        string description, 
        PaymentType paymentType,
        string imagePath)
    {
        Id = Guid.NewGuid();
        DriverId = driverId;
        DriverName = driverName;
        CreatedDate = DateTime.Now.ToUniversalTime();
        UpdatedDate = DateTime.Now.ToUniversalTime();
        ReportDate = reportDate.ToUniversalTime();
        Price = price;
        Description = description;
        PaymentType = paymentType;
        ClientName = clientName;
        ImagePath = imagePath;
        ApplyPaymentRules(paymentType, moneyHolder);
    }

    public void Update(
        DateTime reportDate, 
        decimal price, 
        MoneyHolder moneyHolder, 
        string clientName, 
        string description, 
        PaymentType paymentType,
        string imagePath)
    {
        if (price < 0) throw new InvalidOperationException("Цена не может быть отрицательной");
        if (reportDate> DateTime.Now) throw new InvalidOperationException("Дата не может быть в будущем");
        
        ReportDate = reportDate.ToUniversalTime();
        UpdatedDate = DateTime.Now.ToUniversalTime();
        Price = price;
        Description = description;
        PaymentType = paymentType;
        ClientName = clientName;
        ImagePath = imagePath;
        ApplyPaymentRules(paymentType, moneyHolder);
    }

    public static (Report report, string Error) Create(
        Guid driverId, 
        string driverName, 
        DateTime reportDate, 
        decimal price, 
        MoneyHolder moneyHolder,
        string clientName, 
        string description, 
        PaymentType paymentType,
        string imagePath)
    {
        var error = string.Empty;
        if (string.IsNullOrEmpty(description) || price <= 0 || reportDate> DateTime.Now)
        {
            error = "Описание пустое или дата в будущем или цена невалидная";
        }

        var report = new Report(driverId, reportDate, driverName, price, moneyHolder, clientName, description, paymentType, imagePath);
        return (report, error);
    }

    public void ApplyPaymentRules(PaymentType paymentType, MoneyHolder moneyHolder)
    {
        PaymentType = paymentType;

        if (paymentType == PaymentType.Cash)
        {
            MoneyHolder = moneyHolder; // можно выбрать
        }
        else
        {
            MoneyHolder = MoneyHolder.Victor; // правило!
        }
    }
}