using AutoMapper;
using DriverReports.Application.DTOs.FinancialOperation;
using DriverReports.Application.DTOs.FinancialOperations;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Services.FinancialOperation
{
    public class FinancialOperationsService : IFinancialOperationService
    {
        private readonly IFinancialOperationRepository _financialOpertationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FinancialOperationsService(
            IFinancialOperationRepository financialOpertationRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _financialOpertationRepository = financialOpertationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(
            CreateFinancialOperationDto dto,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var (financialOperation, error) = Domain.Entities.FinancialOperation.Create(
                userId,
                dto.Amount,
                dto.Type,
                dto.Date,
                dto.Comment);

            if (!string.IsNullOrEmpty(error))
                throw new Exception(error);

            await _financialOpertationRepository.AddAsync(financialOperation, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return financialOperation.Id;
        }

        public async Task<IEnumerable<FinancialOperationDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var operations = await _financialOpertationRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<FinancialOperationDto>>(operations);
        }

        public async Task<IEnumerable<FinancialOperationDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var operations = await _financialOpertationRepository.GetByUserIdAsync(userId, cancellationToken);
            return _mapper.Map<IEnumerable<FinancialOperationDto>>(operations);
        }

        public async Task<IEnumerable<FinancialOperationDto>> GetMothlyByUserIdAsync(Guid userId, int year, int month, CancellationToken cancellationToken)
        {
            var reports = await _financialOpertationRepository.GetByUserIdAsync(userId, cancellationToken);
            var rows = reports.Where(r => r.Date.Year == year && r.Date.Month == month);
            var t =  _mapper.Map<IEnumerable<FinancialOperationDto>>(rows);
            return _mapper.Map<IEnumerable<FinancialOperationDto>>(rows);
        }
    }
}
