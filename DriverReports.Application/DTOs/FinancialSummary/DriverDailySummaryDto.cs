using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.DTOs.FinancialSummary
{
    /// <summary>
    /// детализация одного месяца
    /// Это НЕ строка
    // Это контейнер:

    //Месяц
    //  -> список событий
    /// </summary>
    public class DriverDailySummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public Guid DriverId { get; set; }

        public List<DriverDailySummaryRowDto> Rows { get; set; }
    }
}
