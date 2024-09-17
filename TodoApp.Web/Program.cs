using TodoApp.UseCases.Interfaces;
using TodoApp.Infrastructure.Persistense;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using TodoApp.UseCases;
using TodoApp.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.Web;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


var AllowSpecificOrigins = "_AllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
    policy =>
    {
        // @(Victor): Todo: Add the origins that are allowed to access the API
        // policy.WithOrigins([
        //     "http://localhost:5173",
        //     "http://0.0.0.0:5173",
        //     "http://0.0.0.0:80",
        //     "http://localhost:80",
        // ])
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

builder.Services.AddAuthentication(options =>
{
    //  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SigningKey"]))
    };
    options.UseSecurityTokenValidators = true;
});


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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Apply all Database migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Apply pending migrations to the database
    context.Database.Migrate();
}

// app.UseHttpsRedirection();
// app.UseStaticFiles();

app.UseRouting();

app.UseCors(AllowSpecificOrigins);
// app.UseCorsPolicyInspector(); // Add custom middleware to inspect CORS policy

app.UseAuthentication(); // Ensure authentication is used
app.UseAuthorization();  // Ensure authorization is used

app.MapGroup("/api")
    .WithTags("Security")
    .MapIdentityApi<User>();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Urls.Add("http://0.0.0.0:1337");

app.Run();
