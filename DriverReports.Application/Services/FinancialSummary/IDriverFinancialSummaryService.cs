using DriverReports.Application.DTOs.FinancialSummary;
using DriverReports.Application.DTOs.ReportsSummary;

namespace DriverReports.Application.Services.FinancialSummary
{
    public interface IDriverFinancialSummaryService
    {
        /// <summary>
        /// Получить итоговые финансовые отчеты водителя по месяцам.
        /// Одна строка = один месяц.
        Task<List<DriverMonthlySummaryDto>>
            GetDriverMonthlySummaryAsync(
                Guid driverId,
                CancellationToken token);

        /// <summary>
        /// Получить детальную финансовую информацию водителя за месяц.
        /// Одна строка = одно финансовое событие/операция.
        /// </summary>
 
        Task<DriverDailySummaryDto>
            GetDriverMonthlyDetailsAsync(
                Guid driverId,
                int year,
                int month,
                CancellationToken token);

        /// Получить итоговые финансовые отчеты всех водителей за год.
        /// 
        /// Используется администратором.
        /// Одна строка = один водитель за один месяц.
        Task<List<DriverMonthlySummaryDto>>
            GetAllDriversMonthlySummaryAsync(
                int year,
                CancellationToken token);
    }
}