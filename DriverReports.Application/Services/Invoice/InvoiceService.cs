using AutoMapper;
using DriverReports.Application.DTOs.Invoices;
using DriverReports.Application.Interfaces;
using DriverReports.Application.Services.Reports;
using Microsoft.Extensions.Logging;

namespace DriverReports.Application.Services.Invoice
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ILogger<InvoiceService> _logger;
        private readonly IReportRepository _reportRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InvoiceService(
            ILogger<InvoiceService> logger,
            IInvoiceRepository invoiceRepository,
            IReportRepository reportRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _invoiceRepository = invoiceRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(
            CreateInvoiceDto dto,
            CancellationToken cancellationToken)
        {
            var (invoice, error) = Domain.Entities.Invoice.Create(
                dto.Amount,
                dto.InvoiceDate,
                dto.Comment);

            if (!string.IsNullOrEmpty(error))
                throw new Exception(error);

            await _invoiceRepository.AddAsync(invoice, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return invoice.Id;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllAsync(
            int year,
            int month, 
            CancellationToken cancellationToken)
        {
            var operations = await _invoiceRepository.GetAllAsync(year, month, cancellationToken);

            return _mapper.Map<IEnumerable<InvoiceDto>>(operations);
        }

    }
}
