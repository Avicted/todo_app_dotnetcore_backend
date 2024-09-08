using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Services;
using System.Security.Claims;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using TodoApp.Core.Enums;

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
        Post("/api/todos");
        // AllowAnonymous();

        // Document in Swagger that this endpoint returns only the UserName and id
        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Create a new TodoItem";
            s.Description = "The endpoint creates a new TodoItem for the authenticated user.";
            s.ExampleRequest = new TodoItem { Title = "example title", Description = "example description", Status = TodoItemStatus.NotStarted, UserId = "example" };
            s.ResponseExamples[201] = new CreateTodoItemResponseDTO { Id = 1, Title = "example title", Description = "example description", Status = TodoItemStatus.NotStarted, UserId = "example" };
        });
    }

    public override async Task HandleAsync(CreateTodoItemDTO request, CancellationToken ct)
    {
        // Get the user's email from the claims
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        // var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        _logger.LogInformation("CreateTodoItem Retrieving email: {0}", email);

        // If the email is not found, return a 401
        if (email == null)
        {
            _logger.LogError("Email is null");
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
            _logger.LogError("User not found");
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        // Create a new TodoItem
        var todoItem = new CreateTodoItemDTO
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            UserId = user.Id
        };

        // Add the TodoItem
        var res = await _todoItemService.AddTodoItemAsync(todoItem);

        // If the TodoItem is not added, return a 400
        if (res == null)
        {
            _logger.LogError("TodoItem not added");
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
            Status = res.Status,
            UserId = user.Id
        };

        // Return the TodoItem
        await SendAsync(response, StatusCodes.Status201Created, ct);
    }
}
