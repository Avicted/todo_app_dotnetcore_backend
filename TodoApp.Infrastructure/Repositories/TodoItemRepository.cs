using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Persistense;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Infrastructure.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TodoItemRepository> _logger;

    public TodoItemRepository(ApplicationDbContext context, IMapper mapper, ILogger<TodoItemRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetTodoItemByIdResponseDTO?> GetByIdAsync(string userId, int todoItemId)
    {
        // Return the TodoItem that belongs to the user ONLY! use the request.UserId to filter the TodoItem
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == todoItemId);

        return _mapper.Map<GetTodoItemByIdResponseDTO>(todoItem);
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync(string UserId)
    {
        _logger.LogInformation("Retrieving all todo items");
        _logger.LogInformation($"User ID: {UserId}");

        if (string.IsNullOrWhiteSpace(UserId))
        {
            _logger.LogWarning("User ID is null or whitespace.");
            return Enumerable.Empty<TodoItem>();
        }

        // Log the SQL query being executed for debugging purposes
        var sqlQuery = _context.TodoItems.Where(x => x.UserId == UserId).ToQueryString();
        _logger.LogInformation($"Executing SQL Query: {sqlQuery}");

        // Return all TodoItems that belong to the user ONLY! use the request.UserId to filter the TodoItems
        var todoItems = await _context.TodoItems.Where(x => x.UserId == UserId).ToArrayAsync();

        _logger.LogInformation($"Retrieved {todoItems.Length} todo items");

        return todoItems;
    }

    public async Task<CreateTodoItemResponseDTO> AddAsync(CreateTodoItemDTO item)
    {
        var todoItem = _mapper.Map<TodoItem>(item);

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreateTodoItemResponseDTO>(todoItem);
    }

    public async Task<UpdateTodoItemResponseDTO?> UpdateAsync(UpdateTodoItemDTO item)
    {
        var todoItem = await _context.TodoItems.FindAsync(item.Id);
        if (todoItem == null)
        {
            return null;
        }

        // Update the properties of the TodoItem entity
        todoItem.Title = item.Title;
        todoItem.Description = item.Description;
        todoItem.IsCompleted = item.IsCompleted;

        // Save the changes to the database
        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateTodoItemResponseDTO>(item);
    }

    public async Task<DeleteTodoItemResponseDTO?> DeleteAsync(DeleteTodoItemDTO item)
    {
        var todoItem = await _context.TodoItems.FindAsync(item.Id);
        if (todoItem == null)
        {
            return null;
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return new DeleteTodoItemResponseDTO
        {
            Id = todoItem.Id,
        };
    }
}
