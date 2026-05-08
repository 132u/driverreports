using AutoMapper;
using DriverReports.Domain.Entities;
using DriverReports.Application.DTOs.Reports;

namespace DriverReports.Application.Mapping.Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            // =====================================================
            // Entity -> DTO
            // =====================================================
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.ReportDate,
                    opt => opt.MapFrom(src => src.ReportDate))

                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Price))

                .ForMember(dest => dest.MoneyHolder,
                    opt => opt.MapFrom(src => src.MoneyHolder))

                .ForMember(dest => dest.ClientName,
                    opt => opt.MapFrom(src => src.ClientName))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))

                .ForMember(dest => dest.PaymentType,
                    opt => opt.MapFrom(src => src.PaymentType))

                .ForMember(dest => dest.ImagePaths,
                    opt => opt.MapFrom(src => src.ImagePaths));


            // =====================================================
            // DTO -> Entity (если понадобится для создания/обновления)
            // =====================================================

            CreateMap<ReportDto, Report>()
                .ConstructUsing(src =>
                    new Report(
                        Guid.Empty,
                        DateTime.UtcNow,
                        string.Empty,
                        src.Price,
                        src.MoneyHolder,
                        src.ClientName,
                        src.Description,
                        src.PaymentType,
                        src.ImagePaths
                    ));
        }
    }
}