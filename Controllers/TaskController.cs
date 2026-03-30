using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // para ver si el usuario esta autorizado
public class TaskController: ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }

    // metodo para obtener el id del token
    private int GetUserIdFromToken()
    {
        // buscamos el NameIdentifier del ClaimType
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userIdString!);
    }

    // GET: /api/Task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetMyTasks()
    {
        int userId = GetUserIdFromToken();

        // hacemos la busqueda
        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString()
            })
            .ToListAsync();

        return Ok(tasks);
    }

    // POST: /api/Task
    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto request)
    {
        int userId = GetUserIdFromToken();

        // traducimos el dto a nuestro modelo de base de datos
        var newTask = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Status = ItemStatus.Pending, // por defecto se crea como pendiente
            UserId = userId // le asignamos la tarea al usuario del token
        };

        // preparamos el insert
        _context.Tasks.Add(newTask);

        // ejecutamos el insert
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Tarea creada con éxito", TaskId = newTask.Id});
    }

    // PUT: /api/Task/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] ItemStatus newStatus)
    {
        int userId = GetUserIdFromToken();

        // buscamos la tarea pero con la condicion de que pertenezca al usuario del token
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        
        if (task == null)
        {
            return NotFound(new { Message = "Tarea no encontrada o no tienes permisos para editarla."});
        }

        task.Status = newStatus;
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Estado actualizado correctamente"});
    }

    // DELETE: /api/task/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        int userId = GetUserIdFromToken();

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null)
        {
            return NotFound(new { Message = "Tarea no encontrada o no tiene permisos para borrarla."});
        }

        // preparamos el delete
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Tarea eliminada"});
    }

    // GET: /api/task/all
    [HttpGet("all")]
    [Authorize(Roles = "Admin")] // solo el administrador tiene acceso a este endpoint
    public async Task<ActionResult<IEnumerable<object>>> GetAllTasksAdmin()
    {
        // como es el admin entonces no necesitamos filtrar su userId
        var allTasks = await _context.Tasks
            .Include(t => t.User) // para join
            .Select(t => new
            {
                TaskId = t.Id,
                OwnerEmail = t.User.Email,
                Title = t.Title,
                Status = t.Status.ToString()
            })
            .ToListAsync();

        return Ok(allTasks);
    }
}