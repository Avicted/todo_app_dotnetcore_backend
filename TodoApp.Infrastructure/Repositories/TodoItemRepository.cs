using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Persistense;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Interfaces;
using TodoApp.Core.Enums;

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

    public async Task<IEnumerable<TodoItem>> GetAllAsync(string userId)
    {
        var todoItems = await _context.TodoItems
        .Where(x => x.UserId == userId)
        .Select(x => new TodoItem
        {
            Title = x.Title,
            Description = x.Description,
            Status = x.Status,
            UserId = x.UserId,
        })
        .ToListAsync();

        return todoItems;
    }

    public async Task<CreateTodoItemResponseDTO> AddAsync(CreateTodoItemDTO item)
    {
        var todoItem = _mapper.Map<TodoItem>(item);

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreateTodoItemResponseDTO>(todoItem);
    }

    public async Task<UpdateTodoItemResponseDTO?> UpdateAsync(string userId, UpdateTodoItemDTO item)
    {
        // Retrieve the TodoItem entity from the database checking that it belongs to the user
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == item.Id);

        if (todoItem == null)
        {
            return null;
        }

        // Update the properties of the TodoItem entity
        todoItem.Title = item.Title;
        todoItem.Description = item.Description;
        todoItem.Status = item.Status;

        // Save the changes to the database
        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateTodoItemResponseDTO>(item);
    }

    public async Task<DeleteTodoItemResponseDTO?> DeleteAsync(string userId, DeleteTodoItemDTO item)
    {
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == item.Id);

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
