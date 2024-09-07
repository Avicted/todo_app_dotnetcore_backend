using TodoApp.Core.Enums;

namespace TodoApp.Core.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TodoItemStatus Status { get; set; } = TodoItemStatus.NotStarted;

    // Foreign Key for the User who owns this TodoItem
    public string UserId { get; set; } = string.Empty;

    // Navigation property for the related User
    public User User { get; set; } = null!;
}
