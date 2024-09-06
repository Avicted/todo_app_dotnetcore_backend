using System.Security.Claims;
using FastEndpoints;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Web.Endpoints.TodoItems;



public class GetAllTodoItemsEndpoint : Endpoint<GetAllTodoItemsDTO, GetAllTodoItemsResponseDTO>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ILogger<GetAllTodoItemsEndpoint> _logger;

    public GetAllTodoItemsEndpoint(ITodoItemRepository todoItemRepository, ILogger<GetAllTodoItemsEndpoint> logger)
    {
        _todoItemRepository = todoItemRepository;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/api/todos");
        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Retrieve all todo items";
            s.Description = "The endpoint retrieves all todo items from the database";
            s.ExampleRequest = new GetAllTodoItemsDTO();
            s.ResponseExamples[200] = new GetAllTodoItemsResponseDTO
            {
                TodoItems =
                [
                    new TodoItem
                    {
                        Id = 123,
                        Title = "Example Todo Item",
                        Description = "This is an example todo item",
                        IsCompleted = false
                    }
                ]
            };
        });
    }

    public override async Task HandleAsync(GetAllTodoItemsDTO req, CancellationToken ct)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        _logger.LogInformation("Retrieving all todo items");
        _logger.LogInformation($"User ID: {userId}");

        var todoItemsDto = await _todoItemRepository.GetAllAsync(req);

        if (todoItemsDto == null || !todoItemsDto.Any())
        {
            AddError("No todo items found for the user");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        _logger.LogInformation("Todo items retrieved");

        // Map the TodoItem entities to TodoItem DTOs
        var todoItems = todoItemsDto.Select(x => new TodoItem
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            IsCompleted = x.IsCompleted
        });

        await SendAsync(new GetAllTodoItemsResponseDTO { TodoItems = todoItems }, StatusCodes.Status200OK, ct);
    }
}
