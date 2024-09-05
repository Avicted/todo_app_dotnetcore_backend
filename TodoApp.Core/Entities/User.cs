namespace TodoApp.Core.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    // Navigation property for related TodoItems
    public ICollection<TodoItem> TodoItems { get; set; } = [];

    public User()
    {
        Name = string.Empty;
        Password = string.Empty;
        Email = string.Empty;
        TodoItems = [];
    }
}
