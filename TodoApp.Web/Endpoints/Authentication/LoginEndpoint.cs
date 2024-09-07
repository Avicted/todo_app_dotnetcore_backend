using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;

namespace TodoApp.Web.Endpoints.Authentication;

// [HttpPost("/api/custom-login")]
public class CustomLoginEndpoint : Endpoint<LoginRequestDTO, LoginResponseDTO>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomLoginEndpoint> _logger;

    public CustomLoginEndpoint(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration, ILogger<CustomLoginEndpoint> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/api/custom-login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequestDTO req, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);

        _logger.LogInformation($"User found: {user?.Email}");

        if (user == null)
        {
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);

        _logger.LogInformation($"Sign in result: {signInResult.Succeeded}");

        if (!signInResult.Succeeded)
        {
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var token = GenerateJwtToken(user);

        var response = new LoginResponseDTO
        {
            UserId = user.Id,
            Email = user.Email,
            TokenType = "Bearer",
            AccessToken = token,
            ExpiresIn = 3600, // Set to your desired expiration time
            RefreshToken = "not-implemented" // Handle refresh token generation if needed
        };

        await SendAsync(response);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("id", user.Id) // Include user ID as a claim
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // Set to your desired expiration time
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}