using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace TodoApp.Web.Endpoints.Users;

public class GetUserByIdEndpoint : Endpoint<GetUserByIdRequestDTO, GetUserByIdResponseDTO>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<GetUserByIdEndpoint> _logger;

    public GetUserByIdEndpoint(UserManager<User> userManager, ILogger<GetUserByIdEndpoint> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/api/user/{@userId}", r => new { r.Id });

        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Retrieve a user by their ID";
            s.Description = "The endpoint retrieves a user by their ID.";
            s.ExampleRequest = "example_id_123";
            s.ResponseExamples[200] = new GetUserByIdResponseDTO
            {
                Id = "example_id_123",
                Email = "john.doe@domain.tld"
            };
        });
    }


    public override async Task HandleAsync(GetUserByIdRequestDTO res, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(res.Id);

        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var response = new GetUserByIdResponseDTO
        {
            Id = user.Id,
            Email = user.Email
        };

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}
