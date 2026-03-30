namespace TaskManagerApi.DTOs;

// lo que el usuario envia para el registro
public class UserRegistrationDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// lo que el usuario envia para iniciar sesion
public class UserLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// lo que le regresamos si el login es exitoso
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}