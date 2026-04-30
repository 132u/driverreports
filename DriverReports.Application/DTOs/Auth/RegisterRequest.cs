using System;
using System.Collections.Generic;
using System.Text;

namespace DriverReports.Application.DTOs.Auth
{
    public record RegisterRequest(
    string Name,
    string Email,
    string Password);
}
