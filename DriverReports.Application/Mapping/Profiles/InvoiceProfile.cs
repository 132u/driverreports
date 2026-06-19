using AutoMapper;
using DriverReports.Application.DTOs.Invoices;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Mapping.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDto>();
        }
        //public InvoiceProfile()
        //{
        //    // =====================================================
        //    // Entity -> DTO
        //    // =====================================================
        //    CreateMap<Invoice, InvoiceDto>()
        //        .ForMember(dest => dest.Amount,
        //            opt => opt.MapFrom(src => src.Amount))

        //        .ForMember(dest => dest.InvoiceDate,
        //            opt => opt.MapFrom(src => src.InvoiceDate))

        //        .ForMember(dest => dest.Comment,
        //            opt => opt.MapFrom(src => src.Comment));

        //    // =====================================================
        //    // DTO -> Entity (если понадобится для создания/обновления)
        //    // =====================================================

        //    CreateMap<InvoiceDto, Invoice>()
        //        .ConstructUsing(src =>
        //            new Invoice(
        //                Guid.Empty,
        //                src.InvoiceDate,
        //                DateTime.UtcNow,
        //                src.Comment
        //            ));
        //}
    }
}