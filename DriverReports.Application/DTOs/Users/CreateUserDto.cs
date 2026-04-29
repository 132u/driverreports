using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.DTOs.Users
{
    public record CreateUserDto(Guid id, string name, string email, string passwordHash, UserRole role);
}
