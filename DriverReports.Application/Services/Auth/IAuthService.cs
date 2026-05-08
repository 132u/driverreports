using DriverReports.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequest request, CancellationToken token);
        Task<string> Login(LoginRequestDto request, CancellationToken token);
    }
}
