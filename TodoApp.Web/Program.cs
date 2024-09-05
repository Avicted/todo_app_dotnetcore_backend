using Microsoft.EntityFrameworkCore;
using TodoApp.UseCases.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using TodoApp.UseCases.Services; // Namespace for your repository implementation
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.
    AddFastEndpoints().
    SwaggerDocument(); //define a swagger document for the API

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers(); // Adds services for controllers (API)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository implementation
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>(); // Register ITodoItemRepository to its implementation
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Register IUserRepository to its implementation

// Authentication services
builder.Services.AddSingleton<JwtService>();

// Register application services
builder.Services.AddScoped<TodoItemService>(); // Register TodoItemService
builder.Services.AddScoped<UserService>(); // Register UserService

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adds Swagger generation services for FastEndpoints

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication is used
app.UseAuthorization();  // Ensure authorization is used

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
