using DriverReports.Application.DTOs.Auth;
using DriverReports.Application.Interfaces;
using DriverReports.Application.Services.Interfaces;
using DriverReports.Domain.Entities;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(ITokenService tokenService, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> Login(LoginRequest request, CancellationToken token)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return null;

        var valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!valid)
            return null;

        return _tokenService.GenerateToken(user);
    }

    public async Task<string> Register(RegisterRequest request, CancellationToken token)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing != null)
            throw new Exception("User already exists");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(
            Guid.NewGuid(),
            request.Name,
            request.Email,
            passwordHash,
            UserRole.Driver);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(token);
        //TODO: remode debug code
        var rr = _tokenService.GenerateToken(user);
        return _tokenService.GenerateToken(user);
    }
}