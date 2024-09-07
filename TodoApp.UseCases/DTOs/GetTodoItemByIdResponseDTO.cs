using TodoApp.Core.Enums;

namespace TodoApp.UseCases.DTOs;

public class GetTodoItemByIdResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TodoItemStatus Status { get; set; } = TodoItemStatus.NotStarted;

    // Foreign Key for the User who owns this TodoItem
    public string UserId { get; set; } = string.Empty;
}