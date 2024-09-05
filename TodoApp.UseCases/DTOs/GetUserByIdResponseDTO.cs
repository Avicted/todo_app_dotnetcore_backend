namespace TodoApp.UseCases.DTOs;

public class GetUserByIdResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}
