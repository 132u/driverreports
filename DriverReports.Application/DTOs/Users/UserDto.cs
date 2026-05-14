using DriverReports.Domain.Entities;

namespace DriverReports.Application.DTOs.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public UserRole Roles { get; set; }
    }
}
