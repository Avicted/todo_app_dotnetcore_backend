namespace TodoApp.UseCases.DTOs;

// @Note(Victor): We send the Bearertoken in the header, so we don't need to send anything in this body.

public class GetOwnUserDetailsDTO
{
    public string Id { get; set; }
}
