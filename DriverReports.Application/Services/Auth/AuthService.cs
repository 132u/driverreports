using DriverReports.Application.Validators;
using DriverReports.Application.DTOs.Auth;
using DriverReports.Application.Interfaces;
using DriverReports.Application.Services.Auth;
using DriverReports.Application.Services.Reports;
using DriverReports.Domain.Entities;
using Microsoft.Extensions.Logging;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        ILogger<AuthService> logger, 
        ITokenService tokenService, 
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> Login(LoginRequestDto request, CancellationToken token)
    {
        _logger.LogInformation($"Login {request.Email}.");
        var user = await _userRepository.GetByEmailAsync(request.Email, token);
        if (user == null)
            return null;

        var valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!valid)
            return null;


        return _tokenService.GenerateToken(user);
    }

    public async Task<string> Register(RegisterRequest request, CancellationToken token)
    {
        _logger.LogInformation($"Registration {request.Email}, {request.Name}.");
        if (!EmailValidator.IsRussianEmail(request.Email))
        {
            _logger.LogWarning(
              "Registration rejected. Public email domain used: {Email}",
              request.Email);

            throw new ArgumentException($"Регистрация с иностранных почтовых сервисов запрещена {request.Email}.");
        }
        var existing = await _userRepository.GetByEmailAsync(request.Email, token);
        if (existing != null)
            throw new Exception("User already exists");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var (user, error) = User.Create(
            Guid.NewGuid(),
            request.Name,
            request.Email,
            passwordHash,
            UserRole.Driver);

        await _userRepository.AddAsync(user, token);
        await _unitOfWork.SaveChangesAsync(token);
      
        return _tokenService.GenerateToken(user);
    }
}