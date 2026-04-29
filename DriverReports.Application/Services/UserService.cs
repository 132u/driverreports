using DriverReports.Application.DTOs.Reports;
using DriverReports.Application.DTOs.Users;
using DriverReports.Application.Interfaces;
using DriverReports.Application.Services.Interfaces;
using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IReportRepository reportRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken)
        {
            var (user, error) = User.Create(request.id, request.name, request.email, request.passwordHash, request.role);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
