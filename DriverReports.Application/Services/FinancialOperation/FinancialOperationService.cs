using AutoMapper;
using Azure.Core;
using DriverReports.Application.DTOs.FinancialOperation;
using DriverReports.Application.DTOs.FinancialOperations;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DriverReports.Application.Services.FinancialOperation
{
    public class FinancialOperationsService : IFinancialOperationService
    {
        private readonly ILogger<FinancialOperationsService> _logger;
        private readonly IFinancialOperationRepository _financialOpertationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FinancialOperationsService(
            ILogger<FinancialOperationsService> logger,
            IFinancialOperationRepository financialOpertationRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _financialOpertationRepository = financialOpertationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(
            CreateFinancialOperationDto dto,
            Guid userId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"Financial Operation created. User Id={userId}");

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
            _logger.LogInformation("Получить все финансовые операции");
            var operations = await _financialOpertationRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<FinancialOperationDto>>(operations);
        }

        public async Task<IEnumerable<FinancialOperationDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Получить все финансовые операции по UserId = {userId}");
            var operations = await _financialOpertationRepository.GetByUserIdAsync(userId, cancellationToken);
            return _mapper.Map<IEnumerable<FinancialOperationDto>>(operations);
        }

        public async Task<IEnumerable<FinancialOperationDto>> GetMothlyByUserIdAsync(Guid userId, int year, int month, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Получить все финансовые операции за {month} {year} по UserId = {userId}");
            var reports = await _financialOpertationRepository.GetByUserIdAsync(userId, cancellationToken);
            var rows = reports.Where(r => r.Date.Year == year && r.Date.Month == month);
            var t =  _mapper.Map<IEnumerable<FinancialOperationDto>>(rows);
            return _mapper.Map<IEnumerable<FinancialOperationDto>>(rows);
        }
        public async Task DeleteAsync(
            Guid id,
            CancellationToken token)
        {
            _logger.LogWarning(
               "Financial Operation deleted. Id={id}",
               id);
            await _financialOpertationRepository
                .DeleteAsync(id, token);

            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(
            Guid id,
            UpdateFinancialOperationDto request,
            CancellationToken token)
        {
            _logger.LogInformation(
                $"Financial Operation updated. Id={id}");
            var operation = await _financialOpertationRepository
                .GetByIdAsync(id, token);

            if (operation == null)
            {
                throw new KeyNotFoundException("Operation not found");
            }

            operation.Update(
                request.UserId,
                request.Date,
                request.Amount,
                request.Comment
            );

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
