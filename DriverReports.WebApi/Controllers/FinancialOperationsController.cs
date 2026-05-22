using AutoMapper;
using DriverReports.Application.DTOs.FinancialOperation;
using DriverReports.Application.Services.FinancialOperation;
using DriverReports.Application.Services.FinancialSummary;
using DriverReports.Domain.Entities;
using DriverReports.WebApi.Contracts.FinancialOperation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DriverReports.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/financial-operations")]
    public class FinancialOperationsController : BaseController
    {
        private readonly IFinancialOperationService _financialOperationService;
        private readonly IMapper _mapper;
        private readonly IDriverFinancialSummaryService _summaryService;

        public FinancialOperationsController(
            IFinancialOperationService financialOperationService,
            IMapper mapper,
            IDriverFinancialSummaryService summaryService)
        {
            _summaryService = summaryService;
            _financialOperationService = financialOperationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if (IsAdmin)
            {
                var allOperations = await _financialOperationService.GetAllAsync(cancellationToken);
                return Ok(allOperations);
            }

            var operations = await _financialOperationService.GetByUserIdAsync(UserId, cancellationToken);
            return Ok(operations);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFinancialOperationRequest request, CancellationToken token)
        {
            var userId = request.UserId ?? UserId;

            var operationDto = new CreateFinancialOperationDto(request.Date, request.Amount, request.Type, request.Comment);

            var id = await _financialOperationService
                .CreateAsync(operationDto, userId, token);

            return Ok(id);
        }

        /// <summary>
        /// Получить детальную информацию текущего водителя за месяц.
        /// </summary>
        [HttpGet("my/details")]
        public async Task<IActionResult> GetMyMonthlyDetails(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var result = await _summaryService
                .GetDriverMonthlyDetailsAsync(
                    UserId,
                    year,
                    month,
                    token);
            var rows = result.Rows.Where(r => r.Advance != null || r.Fuel != null || r.BaseWork != null);

            return Ok(rows);
        }

        /// <summary>
        /// Получить детальную информацию водителя за месяц.
        /// Только для администратора.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{driverId}/details")]
        public async Task<IActionResult> GetDriverMonthlyDetails(
            Guid driverId,
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken token)
        {
            var result = await _financialOperationService.GetMothlyByUserIdAsync(
                    driverId,
                    year,
                    month,
                    token);
            //var rows = result.Where(r => r.Advance != null || r.Fuel != null || r.BaseWork != null);
            return Ok(result);
        }
    }
}
