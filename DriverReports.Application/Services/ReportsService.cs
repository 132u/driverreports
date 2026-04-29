using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Interfaces;
using DriverReports.Application.Services.Interfaces;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        public ReportsService(IUserRepository userRepository, IReportRepository reportRepository)
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
        }

        public async Task<Guid> CreateReportAsync(CreateReportDto request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByIdAsync(request.UserId);
            if (user == null) {
                throw new Exception("no user");
            }
            var (report , error)= Report.Create(request.UserId, request.Date, request.Price, request.Description, request.PaymentType);
            await _reportRepository.AddAsync(report);
            return report.Id;
        }
    }
}

