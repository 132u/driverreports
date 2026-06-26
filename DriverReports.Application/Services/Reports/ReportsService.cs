using AutoMapper;
using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DriverReports.Application.Services.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly ILogger<ReportsService> _logger;
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReportsService(
            ILogger<ReportsService> logger,
            IUserRepository userRepository,
            IReportRepository reportRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task UpdateAsync(
            UpdateReportDto request,
            Guid id,
            CancellationToken token)
        {
            _logger.LogInformation(
                $"Report updated. ReportId={id}");
            var report = await _reportRepository.GetByIdAsync(id, token);

            if (report == null)
            {
                throw new KeyNotFoundException("Report not found");
            }

            report.Update(
                   request.ReportDate,
                   request.Price,
                   request.MoneyHolder,
                   request.ClientName,
                   request.Description,
                   request.PaymentType,
                   request.ImagePaths
               );
            if (request.DriverId.HasValue)
            {
                var driver = await _userRepository.GetByIdAsync(
                    request.DriverId.Value,
                    token);

                if (driver == null)
                {
                    throw new Exception("Driver not found");
                }

                report.ChangeDriver(
                    driver.Id,
                    driver.Name);
            }

            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(
            Guid id,
            CancellationToken token)
        {
            _logger.LogWarning(
                "Report deleted. ReportId={ReportId}",
                id);
            await _reportRepository.DeleteAsync(id, token);
        }

        public async Task<Guid> CreateAsync(
            CreateReportDto request,
            Guid userId,
            CancellationToken token)
        {
            _logger.LogInformation(
                $"Report created. DriverId={request.DriverId}, Date={request.ReportDate}, Price={request.Price}",
                request.DriverId,
                request.ReportDate,
                request.Price);
            var currentUser = await _userRepository.GetByIdAsync(userId, token);

            if (currentUser == null)
            {
                throw new Exception("User not found");
            }

            var reportDriverId = userId;
            var reportDriverName = currentUser.Name;

            if (currentUser.Roles == UserRole.Admin && request.DriverId.HasValue)
            {
                var driver = await _userRepository
                    .GetByIdAsync(request.DriverId.Value, token);

                if (driver == null)
                {
                    throw new Exception("Driver not found");
                }

                reportDriverId = driver.Id;
                reportDriverName = driver.Name;
            }
            
            var (report, error) = Report.Create(
                reportDriverId,
                reportDriverName,
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


        public async Task<IEnumerable<Report>> GetMothlyByUserIdAsync(Guid userId, int year, int month, CancellationToken cancellationToken)
        {
            var reports = await _reportRepository.GetByUserIdAsync(userId, cancellationToken);
            var rows = reports.Where(r => r.ReportDate.Year == year && r.ReportDate.Month == month).ToList();
            return rows;
        }

        public async Task<IEnumerable<Report>> GetCommonCashlessWithVATMothlyAsync(int year, int month, CancellationToken cancellationToken)
        {
            var reports = await _reportRepository.GetAllAsync(cancellationToken);
            var rows = reports.Where(r => r.ReportDate.Year == year && r.ReportDate.Month == month && r.PaymentType == PaymentType.CashlessWithVAT).ToList();
            return rows;
        }

        public async Task<Report> GetByReportIdAsync(Guid reportId, CancellationToken cancellationToken)
        {
            var reports = await _reportRepository.GetByIdAsync(reportId, cancellationToken);
            return reports;
        }

        public async Task<IEnumerable<Report>> GetDriverMonthlyReportsListAsync(Guid driverId, int year, int month, CancellationToken token)
        {
            var reports = await _reportRepository.GetByUserIdAsync(driverId, token);
            var monthReports = reports.Where(r => r.ReportDate.Year == year && r.ReportDate.Month == month).ToList();

            var rows = monthReports
            .OrderBy(x => x.ReportDate)
            .ToList();

            return rows;
        }
    }
}

