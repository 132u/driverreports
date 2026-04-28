using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.DTOs
{
    public class CreateReportRequest
    {
        public Guid UserId { get; init; }
        public DateTime Date{ get; init; }
        public decimal Price { get; init; }
        public string Description{ get; init; }
        public PaymentType PaymentType{ get; init; }
    }
}
