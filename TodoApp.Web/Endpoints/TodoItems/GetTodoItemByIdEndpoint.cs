using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Web.Endpoints.TodoItems;

public class GetTodoItemByIdEndpoint : Endpoint<GetTodoItemByIdDTO, GetTodoItemByIdResponseDTO>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ILogger<GetTodoItemByIdEndpoint> _logger;

    public GetTodoItemByIdEndpoint(ITodoItemRepository todoItemRepository, ILogger<GetTodoItemByIdEndpoint> logger)
    {
        _todoItemRepository = todoItemRepository;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/api/todo/{@id}", r => new { r.Id });

        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Retrieve a todo item by its ID";
            s.Description = "The endpoint retrieves a todo item by its ID from the database";
            s.ExampleRequest = new GetTodoItemByIdDTO
            {
                Id = 123,
            };
            s.ResponseExamples[200] = new GetTodoItemByIdResponseDTO
            {
                Id = 123,
                Title = "Example Todo Item",
                Description = "This is an example todo item",
                IsCompleted = false
            };
        });
    }

    public override async Task HandleAsync(GetTodoItemByIdDTO req, CancellationToken ct)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(req);

        if (todoItem == null)
        {
            AddError("Todo item not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        await SendAsync(todoItem, StatusCodes.Status200OK, ct);
    }
}