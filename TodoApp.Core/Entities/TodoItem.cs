namespace TodoApp.Core.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    // Foreign Key for the User who owns this TodoItem
    public int UserId { get; set; }

    // Navigation property for the related User
    public User User { get; set; } = null!;
}
