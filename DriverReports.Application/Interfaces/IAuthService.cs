using DriverReports.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequest request, CancellationToken token);
        Task<string> Login(LoginRequest request, CancellationToken token);
    }
}
