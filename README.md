# Dotnet Core minimal Clean Architecture

### Migrations
```bash
# From project root

# Add packages to the projects
cd TodoApp.Web 
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design

# dotnet add package Swashbuckle.AspNetCore.Swagger
# dotnet add package Swashbuckle.AspNetCore
dotnet add package FastEndpoints
dotnet add package FastEndpoints.Swagger

cd TodoApp.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# Creat a migration
dotnet ef migrations add <MigrationName> --startup-project  TodoApp.Web/TodoApp.Web.csproj --project TodoApp.Infrastructure/TodoApp.Infrastructure.csproj

# Update database from within the Web project
cd TodoApp.Web
dotnet ef database update --context ApplicationDbContext --project ../TodoApp.Infrastructure/TodoApp.Infrastructure.csproj --startup-project TodoApp.Web.csproj

```

# Todos
- AutoMapper and DTOs
- Write FluentValidation for all the models
