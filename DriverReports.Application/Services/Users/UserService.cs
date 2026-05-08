using DriverReports.Application.DTOs.Users;
using DriverReports.Application.Interfaces;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Services.Users
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

        public async Task<Guid> CreateUserAsync(CreateUserDto request, CancellationToken token)
        {
            var (user, error) = User.Create(request.id, request.name, request.email, request.passwordHash, request.role);
            await _userRepository.AddAsync(user, token);
            await _unitOfWork.SaveChangesAsync(token);

            return user.Id;
        }

        public Task<IEnumerable<User>> GetUsersAsync(CancellationToken token)
        {
            var result = _userRepository.GetAllAsync(token);
            return result;
        }
    }
}
