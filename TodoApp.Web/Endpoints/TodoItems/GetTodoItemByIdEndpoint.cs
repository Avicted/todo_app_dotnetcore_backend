using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using TodoApp.Core.Entities;
using TodoApp.Core.Enums;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Web.Endpoints.TodoItems;

public class GetTodoItemByIdEndpoint : Endpoint<GetTodoItemByIdDTO, GetTodoItemByIdResponseDTO>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ILogger<GetTodoItemByIdEndpoint> _logger;
    private readonly UserManager<User> _userManager;

    public GetTodoItemByIdEndpoint(ITodoItemRepository todoItemRepository, ILogger<GetTodoItemByIdEndpoint> logger, UserManager<User> userManager)
    {
        _todoItemRepository = todoItemRepository;
        _logger = logger;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get("/api/todos/{@id}", r => new { r.Id });

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
                Status = TodoItemStatus.NotStarted,
            };
        });
    }

    public override async Task HandleAsync(GetTodoItemByIdDTO req, CancellationToken ct)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        _logger.LogInformation("Retrieving todo item by ID");
        _logger.LogInformation($"User userEmail: {userEmail}");

        if (userEmail == null)
        {
            AddError("Unauthorized");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user == null)
        {
            AddError("Unauthorized");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var userId = await _userManager.GetUserIdAsync(user);

        if (userId == null)
        {
            AddError("Unauthorized");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var todoItem = await _todoItemRepository.GetByIdAsync(userId, req.Id);

        if (todoItem == null)
        {
            AddError("Todo item not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        await SendAsync(todoItem, StatusCodes.Status200OK, ct);
    }
}