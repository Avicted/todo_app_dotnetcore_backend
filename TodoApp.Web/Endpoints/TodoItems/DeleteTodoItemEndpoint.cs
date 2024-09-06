using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Web.Endpoints.TodoItems;

public class DeleteTodoItemEndpoint : Endpoint<DeleteTodoItemDTO, DeleteTodoItemResponseDTO>
{
    private readonly ITodoItemRepository _repository;
    private readonly ILogger<DeleteTodoItemEndpoint> _logger;

    public DeleteTodoItemEndpoint(ITodoItemRepository repository, ILogger<DeleteTodoItemEndpoint> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public override void Configure()
    {
        Delete("api/todos/{Id}");
        // AllowAnonymous();

        // Document in Swagger that this endpoint returns only the UserName and id
        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Delete a TodoItem";
            s.Description = "The endpoint deletes a TodoItem for the authenticated user.";
            s.ExampleRequest = new DeleteTodoItemDTO { Id = 1 };
            s.ResponseExamples[200] = new DeleteTodoItemResponseDTO { Id = 1 };
        });
    }

    public override async Task<DeleteTodoItemResponseDTO> HandleAsync(DeleteTodoItemDTO request, CancellationToken cancellationToken)
    {
        var response = await _repository.DeleteAsync(request);

        if (response == null)
        {
            _logger.LogInformation("TodoItem not found");

            AddError("TodoItem not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, cancellationToken);
        }

        _logger.LogInformation("TodoItem deleted: {0}", response?.Id);

        return response!;
    }
}
