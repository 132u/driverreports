using DriverReports.Application.DTOs.ReportsSummary;

namespace DriverReports.Application.Services.FinancialSummary
{
    public class FinancialCalculator
    {
        private const decimal VatRate = 1.22m;

        // -----------------------------
        // 🔹 Конвертация безнала
        // -----------------------------

        /// <summary>
        /// Безнал с НДС → убираем НДС
        /// </summary>
        public decimal ConvertWithVat(decimal amount)
        {
            return amount / VatRate;
        }

        /// <summary>
        /// Безнал без НДС
        /// </summary>
        public decimal ConvertWithoutVat(decimal amount)
        {
            return amount * 0.9m;
        }

        /// <summary>
        /// Общий безнал после конвертации
        /// </summary>
        public decimal CalculateTotalConvertedNonCash(
            decimal nonCashWithVat,
            decimal nonCashWithoutVat)
        {
            return ConvertWithVat(nonCashWithVat)
                 + ConvertWithoutVat(nonCashWithoutVat);
        }

        // -----------------------------
        // 🔹 Зарплата
        // -----------------------------

        /// <summary>
        /// Зарплата = (нал + конвертированный безнал) / 2
        /// </summary>
        public decimal CalculateSalary(
            decimal cashTotal,
            decimal convertedNonCashTotal)
        {
            return (cashTotal + convertedNonCashTotal) / 2m;
        }

        // -----------------------------
        // 🔹 Сколько водитель уже получил
        // -----------------------------

        /// <summary>
        /// Всё, что водитель уже получил:
        /// + аванс
        /// + топливо
        /// + нал
        /// - сдача
        /// - работа на базе
        /// </summary>
        public decimal CalculateDriverReceived(
            decimal cash,
            decimal advance,
            decimal fuel,
            decimal settlement,
            decimal baseWork)
        {
            return cash
                 + advance
                 + fuel
                 - settlement
                 - baseWork;
        }

        // -----------------------------
        // 🔹 Долг
        // -----------------------------

        /// <summary>
        /// > 0  → Виктор должен водителю
        /// < 0  → водитель должен Виктору
        /// </summary>
        public decimal CalculateDebt(
            decimal salary,
            decimal driverReceived)
        {
            return salary - driverReceived;
        }

        // -----------------------------
        // 🔹 Итог месяца (DTO builder)
        // -----------------------------

        public DriverMonthlySummaryDto BuildMonthlySummary(
            int year,
            int month,
            Guid driverId,

            decimal cash,
            decimal nonCashWithVat,
            decimal nonCashWithoutVat,

            decimal advance,
            decimal settlement,
            decimal baseWork,
            decimal fuel)
        {
            var convertedNonCash =
                CalculateTotalConvertedNonCash(nonCashWithVat, nonCashWithoutVat);

            var salary = CalculateSalary(cash, convertedNonCash);

            var driverReceived = CalculateDriverReceived(
                cash,
                advance,
                fuel,
                settlement,
                baseWork);

            var debt = CalculateDebt(salary, driverReceived);

            return new DriverMonthlySummaryDto
            {
                Year = year,
                Month = month,
                //DriverId = driverId,
                //DriverName = driverName,

                CashEarned = cash,
                NonCashWithVat = nonCashWithVat,
                NonCashWithoutVat = nonCashWithoutVat,

                Salary = salary,
                AlreadyPaidToDriver = driverReceived,

                BaseWorkTotal = baseWork,
                AdvanceTotal = advance,
                SettlementsTotal = settlement,
                FuelTotal = fuel,

                TotalEarned = cash + convertedNonCash,

                Balance = debt
            };
        }
    }
}