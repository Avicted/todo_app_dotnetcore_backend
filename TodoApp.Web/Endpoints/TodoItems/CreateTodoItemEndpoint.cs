using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Services;
using System.Security.Claims;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TodoApp.Web.Endpoints.TodoItems;

public class CreateTodoItemEndpoint : Endpoint<CreateTodoItemDTO, CreateTodoItemResponseDTO>
{
    private readonly TodoItemService _todoItemService;
    private readonly ILogger<CreateTodoItemEndpoint> _logger;

    private readonly UserManager<User> _userManager;

    // Inject the logger via the constructor
    public CreateTodoItemEndpoint(TodoItemService todoItemService, ILogger<CreateTodoItemEndpoint> logger, UserManager<User> userManager)
    {
        _todoItemService = todoItemService;
        _logger = logger;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("/api/todo");
        // AllowAnonymous();

        // Document in Swagger that this endpoint returns only the UserName and id
        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Create a new TodoItem";
            s.Description = "The endpoint creates a new TodoItem for the authenticated user.";
            s.ExampleRequest = new TodoItem { Title = "example title", Description = "example description", IsCompleted = false };
            s.ResponseExamples[200] = new CreateTodoItemResponseDTO { Id = 1, Title = "example title", Description = "example description", IsCompleted = false, UserId = "example" };
        });
    }

    public override async Task HandleAsync(CreateTodoItemDTO request, CancellationToken ct)
    {
        // Get the user's email from the claims
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        // If the email is not found, return a 401
        if (email == null)
        {
            AddError("Unauthorized");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        // Get the user from the email
        var user = await _userManager.FindByEmailAsync(email);

        _logger.LogInformation("User found: {0}", user?.Id);

        // If the user is not found, return a 404
        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        // Create a new TodoItem
        var todoItem = new TodoItem
        {
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsCompleted,
            UserId = user.Id,
            User = user
        };

        // Add the TodoItem
        var res = await _todoItemService.AddTodoItemAsync(todoItem);

        // If the TodoItem is not added, return a 400
        if (res == null)
        {
            AddError("TodoItem not added");
            await SendErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        // Log the TodoItem creation
        _logger.LogInformation("TodoItem created: {0}", res.Id);

        var response = new CreateTodoItemResponseDTO
        {
            Id = res.Id,
            Title = res.Title,
            Description = res.Description,
            IsCompleted = res.IsCompleted,
            UserId = user.Id
        };

        // Return the TodoItem
        await SendAsync(response, StatusCodes.Status201Created, ct);
    }
}
