using System.Collections.Generic;
using TodoApp.Core.Entities;

namespace TodoApp.UseCases.DTOs;

public class GetAllTodoItemsResponseDTO
{
    public IEnumerable<TodoItem> TodoItems { get; set; }
}
