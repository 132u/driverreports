using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.FinancialSummary
{
    /// <summary>
    /// одна строка внутри детализации Одна запись = одно событие 
    /// Заявка:
    //01.01
    //Client: BMW
    //Cash: 5000
    /// </summary>
    public class DriverDailySummaryRowDto
    {
        public Guid ReportId { get; set; }
        public DateTime Date { get; set; }

        public string? ClientName { get; set; }

        public decimal? Cash { get; set; }

        public decimal? NonCashWithVat { get; set; }

        public decimal? NonCashWithoutVat { get; set; }

        public decimal? Fuel { get; set; }

        public decimal? Advance { get; set; }

        public decimal? Settlement { get; set; }

        public decimal? BaseWork { get; set; }

        public MoneyHolder? MoneyHolder { get; set; }
    }
}
