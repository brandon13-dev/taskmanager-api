using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    // inyectamos el DbContext y la Configuration
    public AuthService(AppDbContext context, IConfiguration configuration)
    {
     _context = context;
     _configuration = configuration;   
    }

    public async Task<AuthResponseDto> RegisterAsync(UserRegistrationDto request)
    {
        // verificamos si el usuario ya existe
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return new AuthResponseDto { Message = "El correo ya esta registrado."};
        }

        // Hasheamos la contrasena
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // creamos el nuevo usuario
        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = "User" // por defecto user
        };

        // gurardamos en la base de datos
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponseDto { Message = "Usuario registrado exitosamente."};
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto request)
    {
        // buscamos el usuario por email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        // si no existe o la contrasena no coincide
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new AuthResponseDto { Message = "Credenciales incorrectas."};
        }

        // si todo esta bien, fabricamos el token
        string token = CreateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Message = "Login exitoso."
        };
    }

    // metodo para crear el token
    private string CreateToken(User user)
    {
        // Claims
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // sacamos la clave secreta del appsetings
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));

        // elegimos el algoritmo de encriptacion
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        // ensamblamos el token final
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),//caduca en un dia
            signingCredentials: creds
        );

        // lo convertimos a string para enviarselo al usuario
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}