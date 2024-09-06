using Microsoft.EntityFrameworkCore;
using TodoApp.UseCases.Interfaces;
using TodoApp.Infrastructure.Persistense;
using TodoApp.Infrastructure.Repositories;
using TodoApp.UseCases.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using TodoApp.UseCases;
using TodoApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.
  AddFastEndpoints().
  SwaggerDocument(); //define a swagger document for the API

// Add services to the container.
builder.Services
    .AddAuthorization()
    .AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDataProtection();
builder.Services.AddControllers(); // Adds services for controllers (API)

// Dependency injection for infrastructure, Register infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Register repository implementation
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// Dependency injection for use cases
builder.Services.AddUseCases(); // Register use cases

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

app.MapGroup("/api").WithTags("Security").MapIdentityApi<User>();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
