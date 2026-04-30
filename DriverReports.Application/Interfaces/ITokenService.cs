using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
