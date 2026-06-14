using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverReports.Application.DTOs.Invoices
{
    public record InvoiceDto
    (
        decimal Amount,
        DateTime InvoiceDate,
        string? Comment
    );
}
