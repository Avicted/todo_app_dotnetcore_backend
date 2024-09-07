using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace TodoApp.Web.Endpoints.Users;

public class GetOwnUserDetailsEndpoint : Endpoint<GetOwnUserDetailsDTO, GetOwnUserDetailsResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<GetUserByIdEndpoint> _logger;

    public GetOwnUserDetailsEndpoint(UserManager<User> userManager, ILogger<GetUserByIdEndpoint> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/api/users/getowndetails");

        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Retrieve own user details";
            s.Description = "The endpoint retrieves a user by their Bearer Token.";
            s.ResponseExamples[200] = new GetOwnUserDetailsResponse
            {
                UserId = "example_id_123",
                Email = "kalle.anka@domain.se",
                TokenType = "Bearer",
                AccessToken = "example_access_token",
                ExpiresIn = 3600,
                RefreshToken = "example_refresh_token"
            };
        });
    }


    public override async Task HandleAsync(GetOwnUserDetailsDTO res, CancellationToken ct)
    {
        var userId = res.Id;

        var user = await _userManager.FindByIdAsync(userId);

        _logger.LogInformation("User: {0}", user);

        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var response = new GetOwnUserDetailsResponse
        {
            UserId = user.Id,
            Email = user.Email,
            TokenType = "Bearer",
            AccessToken = "example_access_token",
            ExpiresIn = 3600,
            RefreshToken = "example_refresh_token"
        };

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}
