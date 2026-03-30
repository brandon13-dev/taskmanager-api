namespace TaskManagerApi.Models;

public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "Admin" o "User"
        
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }