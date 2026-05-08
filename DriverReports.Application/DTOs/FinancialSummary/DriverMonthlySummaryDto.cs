using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.ReportsSummary
{
    /// Одна строка = один месяц. итог одного месяца для одного водителя
    /// главная summary таблица
    /// Январь 2026
//Водитель: Вася

//Всего заработано: 300000
//Зарплата: 150000
//Получил: 120000
//Долг: 30000
    public class DriverMonthlySummaryDto
    {
        public int Year { get; set; }

        public int Month { get; set; }

        /// <summary>
        /// Всего заработано
        /// Нал + безнал после конвертации
        /// </summary>
        public decimal TotalEarned { get; set; }

        /// <summary>
        /// Наличные деньги
        /// </summary>
        public decimal CashEarned { get; set; }

        /// <summary>
        /// Безналичные с НДС
        /// </summary>
        public decimal NonCashWithVat { get; set; }

        /// <summary>
        /// Безналичные без НДС
        /// </summary>
        public decimal NonCashWithoutVat { get; set; }

        /// <summary>
        /// Зарплата водителя
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Сколько водитель уже получил
        /// </summary>
        public decimal DriverReceived { get; set; }

        /// <summary>
        /// Работа на базе
        /// </summary>
        public decimal BaseWorkTotal { get; set; }

        /// <summary>
        /// Авансы
        /// </summary>
        public decimal AdvanceTotal { get; set; }

        /// <summary>
        /// Топливо
        /// </summary>
        public decimal FuelTotal { get; set; }

        /// <summary>
        /// Сколько водитель сдал Виктору
        /// </summary>
        public decimal SettlementsTotal { get; set; }

        /// <summary>
        /// Кто кому сколько должен
        /// > 0  -> Виктор должен водителю
        /// < 0  -> водитель должен Виктору
        /// </summary>
        public decimal RemainingDebt { get; set; }
    }
}
