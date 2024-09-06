namespace TodoApp.UseCases.DTOs;

public class CreateTodoItemDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}