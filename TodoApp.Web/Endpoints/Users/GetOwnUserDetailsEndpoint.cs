using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TodoApp.Web.Endpoints.Users;

public class GetOwnUserDetailsEndpoint : Endpoint<EmptyRequest, GetOwnUserDetailsResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<GetOwnUserDetailsEndpoint> _logger;

    public GetOwnUserDetailsEndpoint(UserManager<User> userManager, ILogger<GetOwnUserDetailsEndpoint> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/api/users/getowndetails");

        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Retrieve own user details";
            s.Description = "The endpoint retrieves a user by their Bearer Token.";
            s.ResponseExamples[200] = new GetOwnUserDetailsResponse
            {
                UserId = "example_id_123",
                Email = "kalle.anka@domain.se",
            };
        });
    }

    public override async Task HandleAsync(EmptyRequest _emptyReq, CancellationToken ct)
    {
        // Get the user's email from the claims
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        // If the email is not found, return a 401
        /*if (email == null)
        {
            AddError("Unauthorized");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }*/

        // Get the user from the email
        var user = await _userManager.FindByEmailAsync(email);

        _logger.LogInformation("GetOwnUserDetailsEndpoint: User found: {0}", user?.Id);

        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var response = new GetOwnUserDetailsResponse
        {
            UserId = user.Id,
            Email = user.Email
        };

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}
