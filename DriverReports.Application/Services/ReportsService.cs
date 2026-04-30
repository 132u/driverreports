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
        private readonly IUnitOfWork _unitOfWork;
        public ReportsService(IUserRepository userRepository, IReportRepository reportRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateReportAsync(CreateReportDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) {
                throw new Exception("no user");
            }
            var (report , error)= Report.Create(request.UserId, request.Date, request.Price, request.Description, request.PaymentType);
            await _reportRepository.AddAsync(report);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return report.Id;
        }

        public async Task<IEnumerable<Report>> GetReportsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _reportRepository.GetByDriverIdAsync(userId);

            return result;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync(CancellationToken cancellationToken)
        {
            var result = await _reportRepository.GetAllAsync();

            return result;
        }
    }
}

