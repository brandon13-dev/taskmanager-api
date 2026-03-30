namespace TaskManagerApi.Models;

public enum ItemStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        
        public ItemStatus Status { get; set; } = ItemStatus.Pending;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

        // Relación con User
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }