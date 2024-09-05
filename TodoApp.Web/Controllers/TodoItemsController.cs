/* using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Entities;
using TodoApp.UseCases.Services;

namespace TodoApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly TodoItemService _todoItemService;

    public TodoItemsController(TodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItemById(int id)
    {
        var todoItem = await _todoItemService.GetTodoItemByIdAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return Ok(todoItem);
    }

    // POST: api/TodoItems
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        await _todoItemService.AddTodoItemAsync(todoItem);
        return CreatedAtAction(nameof(GetTodoItemById), new { id = todoItem.Id }, todoItem);
    }

    // PUT: api/TodoItems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodoItem(int id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        await _todoItemService.UpdateTodoItemAsync(todoItem);

        return NoContent();
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        await _todoItemService.DeleteTodoItemAsync(id);
        return NoContent();
    }
}
*/