
using TodoApp.Core.Enums;

namespace TodoApp.UseCases.DTOs;

public class UpdateTodoItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TodoItemStatus Status { get; set; } = TodoItemStatus.NotStarted;
}