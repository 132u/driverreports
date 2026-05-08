using AutoMapper;
using DriverReports.Application.DTOs.FinancialOperations;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Mapping.Profiles
{
    public class FinancialOperationProfile : Profile
    {
        public FinancialOperationProfile()
        {
            CreateMap<FinancialOperation, FinancialOperationDto>();
        }
    }
}
