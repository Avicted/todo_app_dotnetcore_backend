namespace TodoApp.UseCases.DTOs;

// @Note(Victor) This is exactly TodoApp.UseCases/DTOs/LoginResponseDTO.cs:
public class GetOwnUserDetailsResponse
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}
