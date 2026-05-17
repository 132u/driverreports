using AutoMapper;
using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Services.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReportsService(
            IUserRepository userRepository, 
            IReportRepository reportRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(CreateReportDto request, Guid userId, CancellationToken token)
        {
            var user = await _userRepository.GetByIdAsync(userId, token);

            if (user == null) {
                throw new Exception("no user");
            }

            var (report, error) = Report.Create(
                userId,
                user.Name,
                request.ReportDate,
                request.Price,
                request.MoneyHolder,
                request.ClientName,
                request.Description,
                request.PaymentType,
                request.ImagePaths);


            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            await _reportRepository.AddAsync(report, token);
            await _unitOfWork.SaveChangesAsync(token);

            return report.Id;
        }

        public async Task<IEnumerable<Report>> GetAllAsync(CancellationToken cancellationToken)
        {
            var reports = await _reportRepository.GetAllAsync(cancellationToken);
            return reports;
        }

        public async Task<IEnumerable<Report>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var reports = await _reportRepository.GetByUserIdAsync(userId, cancellationToken);
            return reports;
        }

        public async Task<Report> GetByReportIdAsync(Guid reportId, CancellationToken cancellationToken)
        {
            var reports = await _reportRepository.GetByIdAsync(reportId, cancellationToken);
            return reports;
        }
    }
}

