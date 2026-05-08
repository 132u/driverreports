using DriverReports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services.Auth
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
