using AutoMapper;
using DriverReports.Application.DTOs.Invoices;
using DriverReports.Application.Interfaces;

namespace DriverReports.Application.Services.Invoice
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
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
