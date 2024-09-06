using Microsoft.AspNetCore.Identity;


namespace TodoApp.Core.Entities;

public class User : IdentityUser
{
    // Navigation property for related TodoItems
    public ICollection<TodoItem> TodoItems { get; set; } = [];

    public User()
    {
        TodoItems = [];
    }
}
