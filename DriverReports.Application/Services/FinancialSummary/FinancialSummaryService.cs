using Azure.Core;
using DriverReports.Application.DTOs.FinancialSummary;
using DriverReports.Application.DTOs.ReportsSummary;
using DriverReports.Application.Interfaces;
using DriverReports.Application.Services.Reports;
using DriverReports.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DriverReports.Application.Services.FinancialSummary
{
    public class SummaryService : ISummaryService
    {
        private readonly ILogger<SummaryService> _logger;
        private readonly IReportRepository _reportRepository;
        private readonly IFinancialOperationRepository _financialRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly FinancialCalculator _calculator;
            
        public SummaryService(
            ILogger<SummaryService> logger,
            IReportRepository reportRepository,
            IUserRepository userRepository,
            IFinancialOperationRepository financialRepository,
            IInvoiceRepository invoiceRepository,
            FinancialCalculator calculator)
        {
            _logger = logger;
            _reportRepository = reportRepository;
            _userRepository = userRepository;
            _invoiceRepository = invoiceRepository;
            _financialRepository = financialRepository;
            _calculator = calculator;
        }

         public async Task<DriverMonthlySummaryDto> GetSummaryAsync(
            Guid driverId,
            int year,
            int month,
            CancellationToken token)
        {
            _logger.LogInformation($"GetSummaryAsync driverId ={driverId} {month} {year}.");
            var reports =
                await _reportRepository
                    .GetByUserIdAsync(driverId, token);

            var operations =
                await _financialRepository
                    .GetByUserIdAsync(driverId, token);

            var monthReports = reports
                .Where(x =>
                    x.ReportDate.Year == year &&
                    x.ReportDate.Month == month)
                .ToList();

            var monthOperations = operations
                .Where(x =>
                    x.Date.Year == year &&
                    x.Date.Month == month)
                .ToList();

            // =========================
            // REPORTS
            // =========================
            var monthReportsFilter = monthReports
                            .Where(x => x.PaymentType == PaymentType.Cash);


            var cash = monthReportsFilter
                .Sum(x => x.Price);

            var driverMoney = monthReportsFilter.Where(r=>r.MoneyHolder == MoneyHolder.Driver).Sum(x => x.Price);
            var viktorMoney = monthReportsFilter.Where(r => r.MoneyHolder == MoneyHolder.Victor).Sum(x => x.Price);

            var nonCashWithVat = monthReports
                .Where(x => x.PaymentType == PaymentType.CashlessWithVAT)
                .Sum(x => x.Price);

            var nonCashWithoutVat = monthReports
                .Where(x => x.PaymentType == PaymentType.CashlessWithoutVAT)
                .Sum(x => x.Price);

            // =========================
            // OPERATIONS
            // =========================

            var advance = monthOperations
                .Where(x => x.Type == FinancialOperationType.Advance)
                .Sum(x => x.Amount);

            var settlement = monthOperations
                .Where(x => x.Type == FinancialOperationType.Settlement)
                .Sum(x => x.Amount);

            var fuel = monthOperations
                .Where(x => x.Type == FinancialOperationType.FuelExpense)
                .Sum(x => x.Amount);

            var baseWork = monthOperations
                .Where(x => x.Type == FinancialOperationType.BaseWorkPayment)
                .Sum(x => x.Amount);

            // =========================
            // CALCULATOR
            // =========================

            return _calculator.BuildMonthlySummary(
                year,
                month,
                driverId,
                cash,
                nonCashWithVat,
                nonCashWithoutVat,
                advance,
                settlement,
                baseWork,
                fuel,
                driverMoney,
                viktorMoney);
        }

        /// Получить итоговые финансовые отчеты всех водителей за год.
        /// Используется администратором.
        /// Одна строка = один водитель за один месяц.
        public async Task<List<DriverMonthlySummaryDto>>
    GetAllDriversMonthlySummaryAsync(
        int year,
        CancellationToken token)
        {
            _logger.LogInformation($"GetAllDriversMonthlySummaryAsync Получить итоговые финансовые отчеты всех водителей за год = {year}.");
            // ---------------------------------
            // Reports
            // ---------------------------------
            var reports = await _reportRepository
                .GetAllAsync(token);

            reports = reports
                .Where(x => x.ReportDate.Year == year)
                .ToList();

            // ---------------------------------
            // Financial operations
            // ---------------------------------
            var operations = await _financialRepository
                .GetAllAsync(token);

            operations = operations
                .Where(x => x.Date.Year == year)
                .ToList();

            // ---------------------------------
            // Group by driver + month
            // ---------------------------------
            var groupedReports = reports
                .GroupBy(x => new
                {
                    x.DriverId,
                    x.DriverName,
                    x.ReportDate.Year,
                    x.ReportDate.Month
                })
                .OrderBy(x => x.Key.DriverName)
                .ThenBy(x => x.Key.Month);

            var result = new List<DriverMonthlySummaryDto>();

            foreach (var group in groupedReports)
            {
                var driverId = group.Key.DriverId;
                var month = group.Key.Month;

                // ---------------------------------
                // Reports
                // ---------------------------------
                var monthReports = group.ToList();

                // ---------------------------------
                // Operations
                // ---------------------------------
                var monthOperations = operations
                    .Where(x =>
                        x.UserId == driverId &&
                        x.Date.Month == month)
                    .ToList();

                // =====================================================
                // Earnings
                // =====================================================

                var cashEarned = monthReports
                    .Where(x => x.PaymentType == PaymentType.Cash)
                    .Sum(x => x.Price);

                var nonCashWithVat = monthReports
                    .Where(x => x.PaymentType == PaymentType.CashlessWithVAT)
                    .Sum(x => x.Price);

                var nonCashWithoutVat = monthReports
                    .Where(x => x.PaymentType == PaymentType.CashlessWithoutVAT)
                    .Sum(x => x.Price);

                // =====================================================
                // Operations totals
                // =====================================================

                var advanceTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.Advance)
                    .Sum(x => x.Amount);

                var settlementTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.Settlement)
                    .Sum(x => x.Amount);

                var fuelTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.FuelExpense)
                    .Sum(x => x.Amount);

                var baseWorkTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.BaseWorkPayment)
                    .Sum(x => x.Amount);

                // =====================================================
                // Calculator
                // =====================================================

                var convertedNonCash =
                    _calculator.CalculateTotalConvertedNonCash(
                        nonCashWithVat,
                        nonCashWithoutVat);

                var totalEarned =
                    cashEarned + convertedNonCash;

                var salary =
                    _calculator.CalculateSalary(
                        cashEarned,
                        convertedNonCash);

                var driverReceived =
                    _calculator.CalculateDriverReceived(
                        cashEarned,
                        advanceTotal,
                        fuelTotal,
                        settlementTotal,
                        baseWorkTotal);

                var debt =
                    _calculator.CalculateDebt(
                        salary,
                        driverReceived);

                // =====================================================
                // DTO
                // =====================================================

                result.Add(new DriverMonthlySummaryDto
                {
                    Year = year,
                    Month = month,

                    //DriverId = driverId,
                    //DriverName = group.Key.DriverName,

                    TotalEarned = totalEarned,

                    CashEarned = cashEarned,

                    NonCashWithVat = nonCashWithVat,

                    NonCashWithoutVat = nonCashWithoutVat,

                    Salary = salary,

                    AlreadyPaidToDriver = driverReceived,

                    BaseWorkTotal = baseWorkTotal,

                    AdvanceTotal = advanceTotal,

                    FuelTotal = fuelTotal,

                    SettlementsTotal = settlementTotal,

                    Balance = debt
                });
            }

            return result;
        }

        /// <summary>
        /// Получить детальную финансовую информацию водителя за месяц.
        /// Одна строка = одно финансовое событие/операция.
        /// </summary>
        public async Task<DriverDailySummaryDto> GetDriverMonthlyDetailsAsync(Guid driverId, int year, int month, CancellationToken token)
        {
            _logger.LogInformation($"GetDriverMonthlyDetailsAsync детальную финансовую информацию водителя за месяц driverId = {driverId} {month} {year}.");
            var reports = await _reportRepository.GetByUserIdAsync(driverId, token);
            var monthReports = reports.Where(r => r.ReportDate.Year == year && r.ReportDate.Month == month).ToList();
            var operations = await _financialRepository.GetByUserIdAsync(driverId, token);
            var monthOperations = operations.Where(f => f.Date.Year == year && f.Date.Month == month).ToList();
            var rows = new List<DriverDailySummaryRowDto>();
            foreach (var report in monthReports) 
            {
                rows.Add(new DriverDailySummaryRowDto()
                {
                    ReportId = report.Id,
                    Date = report.ReportDate,
                    ClientName = report.ClientName,
                    Cash =
                report.PaymentType == PaymentType.Cash
                    ? report.Price
                    : null,

                    NonCashWithVat =
                report.PaymentType == PaymentType.CashlessWithVAT
                    ? report.Price
                    : null,

                    NonCashWithoutVat =
                report.PaymentType == PaymentType.CashlessWithoutVAT
                    ? report.Price
                    : null,

                    MoneyHolder = report.MoneyHolder,
                }); 
            }

            foreach (var operation in monthOperations) 
            {
                rows.Add(new DriverDailySummaryRowDto()
                {
                    Date = operation.Date,
                    Advance = operation.Type == FinancialOperationType.Advance ? operation.Amount : null,
                    Settlement = operation.Type == FinancialOperationType.Settlement ? operation.Amount : null,
                    BaseWork = operation.Type == FinancialOperationType.BaseWorkPayment ? operation.Amount : null,
                    Fuel = operation.Type == FinancialOperationType.FuelExpense ? operation.Amount : null,
                });
            }

            rows = rows
            .OrderBy(x => x.Date)
            .ToList();

            return new DriverDailySummaryDto
            {
                Year = year,
                Month = month,
                DriverId = driverId,
                Rows = rows
            };
        }

        /// <summary>
        /// Получить итоговые финансовые отчеты водителя по месяцам.
        /// Одна строка = один месяц.
        public async Task<List<DriverMonthlySummaryDto>> GetDriverMonthlySummaryAsync(Guid driverId, CancellationToken token)
        {
            _logger.LogInformation($"GetDriverMonthlySummaryAsync Получить итоговые финансовые отчеты водителя по месяцам driverId = {driverId}.");
            // ---------------------------------
            // Reports
            // ---------------------------------
            var reports = await _reportRepository
                .GetByUserIdAsync(driverId, token);

            // ---------------------------------
            // Financial operations
            // ---------------------------------
            var operations = await _financialRepository
                .GetByUserIdAsync(driverId, token);

            // ---------------------------------
            // Group reports by month
            // ---------------------------------
            var monthGroups = reports
                .GroupBy(x => new
                {
                    x.ReportDate.Year,
                    x.ReportDate.Month
                })
                .OrderBy(x => x.Key.Year)
                .ThenBy(x => x.Key.Month);

            var result = new List<DriverMonthlySummaryDto>();

            foreach (var group in monthGroups)
            {
                var year = group.Key.Year;
                var month = group.Key.Month;

                // -----------------------------
                // Reports for month
                // -----------------------------
                var monthReports = group.ToList();

                // -----------------------------
                // Operations for month
                // -----------------------------
                var monthOperations = operations
                    .Where(x =>
                        x.Date.Year == year &&
                        x.Date.Month == month)
                    .ToList();

                // =====================================================
                // Earnings
                // =====================================================

                var cashEarned = monthReports
                    .Where(x => x.PaymentType == PaymentType.Cash)
                    .Sum(x => x.Price);

                var nonCashWithVat = monthReports
                    .Where(x => x.PaymentType == PaymentType.CashlessWithVAT)
                    .Sum(x => x.Price);

                var nonCashWithoutVat = monthReports
                    .Where(x => x.PaymentType == PaymentType.CashlessWithoutVAT)
                    .Sum(x => x.Price);

                // =====================================================
                // Financial operations
                // =====================================================

                var advanceTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.Advance)
                    .Sum(x => x.Amount);

                var settlementTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.Settlement)
                    .Sum(x => x.Amount);

                var fuelTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.FuelExpense)
                    .Sum(x => x.Amount);

                var baseWorkTotal = monthOperations
                    .Where(x => x.Type == FinancialOperationType.BaseWorkPayment)
                    .Sum(x => x.Amount);

                // =====================================================
                // Calculator
                // =====================================================

                var convertedNonCash =
                    _calculator.CalculateTotalConvertedNonCash(
                        nonCashWithVat,
                        nonCashWithoutVat);

                var totalEarned =
                    cashEarned + convertedNonCash;

                var salary =
                    _calculator.CalculateSalary(
                        cashEarned,
                        convertedNonCash);

                var driverReceived =
                    _calculator.CalculateDriverReceived(
                        cashEarned,
                        advanceTotal,
                        fuelTotal,
                        settlementTotal,
                        baseWorkTotal);

                var debt =
                    _calculator.CalculateDebt(
                        salary,
                        driverReceived);

                // =====================================================
                // DTO
                // =====================================================

                result.Add(new DriverMonthlySummaryDto
                {
                    Year = year,
                    Month = month,

                    TotalEarned = totalEarned,

                    CashEarned = cashEarned,

                    NonCashWithVat = nonCashWithVat,

                    NonCashWithoutVat = nonCashWithoutVat,

                    Salary = salary,

                    AlreadyPaidToDriver = driverReceived,

                    BaseWorkTotal = baseWorkTotal,

                    AdvanceTotal = advanceTotal,

                    FuelTotal = fuelTotal,

                    SettlementsTotal = settlementTotal,

                    Balance = debt
                });
            }

            return result;
        }

        public async Task<decimal> GetCashlessWithVatTotalAsync(int year, int month, CancellationToken token)
        {
            return await _reportRepository.GetCashlessWithVatTotalAsync(year, month, token);
        }

        public async Task<decimal> GetTotalInvoicesAmountAsync(int year, int month, CancellationToken token)
        {
            return await _invoiceRepository.GetTotalInvoicesAmountAsync(year, month, token);
        }

        public decimal CalculateCashlessVATBalance(decimal cashlessVATAmount, decimal invoicesAmount, CancellationToken token)
        {
            return _calculator.CalculateCashlessVATBalance(cashlessVATAmount, invoicesAmount);
        }
    }
}
