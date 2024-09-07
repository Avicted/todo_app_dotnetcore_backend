namespace TodoApp.UseCases.DTOs;

public class LoginResponseDTO
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}
