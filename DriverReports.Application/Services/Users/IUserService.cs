using DriverReports.Application.DTOs.Users;
using DriverReports.Domain.Entities;

namespace DriverReports.Application.Services.Users
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(
             CreateUserDto request,
             CancellationToken cancellationToken);

        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);

        Task<List<UserDto>> GetDriversAsync(CancellationToken token);
    }
}
