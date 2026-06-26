namespace DriverReports.Domain.Entities;

public enum PaymentType
{
    Cash,
    CashlessWithVAT,
    CashlessWithoutVAT
}

public enum MoneyHolder
{
    Driver = 0,
    Victor = 1
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
    
    public List<string?> ImagePaths { get; set; } = new();

    public string ClientName { get; private set; }

    public Report()
    { }

    public Report(
        Guid driverId, 
        DateTime reportDate, 
        string driverName, 
        decimal price, 
        MoneyHolder moneyHolder, 
        string clientName, 
        string description, 
        PaymentType paymentType,
        List<string> imagePaths)
    {
        Id = Guid.NewGuid();
        DriverId = driverId;
        DriverName = driverName;
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
        ReportDate = DateTime.SpecifyKind(reportDate.Date, DateTimeKind.Utc); ;
        Price = price;
        Description = description;
        PaymentType = paymentType;
        ClientName = clientName;
        ImagePaths = imagePaths ?? new List<string>();
        ApplyPaymentRules(paymentType, moneyHolder);
    }
    public void ChangeDriver(
    Guid driverId,
    string driverName)
    {
        DriverId = driverId;
        DriverName = driverName;
    }

    public void Update(
        DateTime reportDate, 
        decimal price, 
        MoneyHolder moneyHolder, 
        string clientName, 
        string description, 
        PaymentType paymentType,
        List<string> imagePath)
    {
        if (price < 0) throw new InvalidOperationException("Цена не может быть отрицательной");
        if (reportDate > DateTime.Now) throw new InvalidOperationException("Дата не может быть в будущем");
        
        ReportDate = DateTime.SpecifyKind(
    reportDate.Date,
    DateTimeKind.Utc);
        UpdatedDate = DateTime.UtcNow;
        Price = price;
        Description = description;
        PaymentType = paymentType;
        ClientName = clientName;
        ImagePaths = imagePath;
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
        List<string> imagePath)
    {
        var error = string.Empty;
        if (string.IsNullOrEmpty(description) || price <= 0 || reportDate.Date > DateTime.Today)
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