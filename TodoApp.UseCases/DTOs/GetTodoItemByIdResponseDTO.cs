namespace TodoApp.UseCases.DTOs;

public class GetTodoItemByIdResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    // Foreign Key for the User who owns this TodoItem
    public string UserId { get; set; } = string.Empty;
}