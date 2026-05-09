using AutoMapper;
using DriverReports.Application.DTOs.FinancialOperation;
using DriverReports.Application.Services.FinancialOperation;
using DriverReports.WebApi.Contracts.FinancialOperation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DriverReports.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialOperationsController : BaseController
    {
        private readonly IFinancialOperationService _financialOperationService;
        private readonly IMapper _mapper;

        public FinancialOperationsController(
            IFinancialOperationService financialOperationService,
            IMapper mapper)
        {
            _financialOperationService = financialOperationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if(IsAdmin)
            {
                 var allOperations= await _financialOperationService.GetAllAsync(cancellationToken);
                return Ok(allOperations);
            }

            var operations = await _financialOperationService.GetByUserIdAsync(UserId, cancellationToken);
            return Ok(operations);

        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateFinancialOperationRequest request, CancellationToken token)
        {
            var operationDto = new CreateFinancialOperationDto(request.Date, request.Amount, request.Type, request.Comment);

            var id = await _financialOperationService.CreateAsync(operationDto, UserId, token);
            return Ok(id);
        }
    }
}
