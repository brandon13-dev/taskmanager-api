using TaskManagerApi.Models;

namespace TaskManagerApi.DTOs;

// lo que el usuario envia para crear una nueva tarea
public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public PriorityLevel Priority { get; set; }
}

// lo que el usuario recibe
public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
}