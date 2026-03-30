using TaskManagerApi.DTOs;

namespace TaskManagerApi.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(UserRegistrationDto request);
    Task<AuthResponseDto> LoginAsync(UserLoginDto request);
}