using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers;

[Route("api/[controller]")] // url -> /api/auth
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // post: /api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(UserRegistrationDto request)
    {
        var response = await _authService.RegisterAsync(request);

        if (response.Message == "El correo ya esta registrado.")
        {
            return BadRequest(response); // devuelve error 400
        }

        return Ok(response); // devuelve 200 ok
    }

    // post: /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(UserLoginDto request)
    {
        var response = await _authService.LoginAsync(request);

        if (response.Message == "Credenciales incorrectas.")
        {
            return Unauthorized(response); // devuelve un error 401
        }

        return Ok(response); // devuelve el token y un 200 ok
    }
}