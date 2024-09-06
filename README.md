# Dotnet Core minimal Clean Architecture

## Work in progress!

### Initial setup, already done
```bash
# From project root

# Add packages to the projects
cd TodoApp.Web 
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
# dotnet add package Swashbuckle.AspNetCore.Swagger
# dotnet add package Swashbuckle.AspNetCore
dotnet add package FastEndpoints
dotnet add package FastEndpoints.Swagger
 dotnet add package FastEndpoints.Security


cd TodoApp.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.IdentityModel
dotnet add package System.IdentityModel.Tokens.Jwt


cd TodoApp.Core
dotnet add package Microsoft.AspNetCore.Identity
```

### Migrations
```bash
# Creat a migration
dotnet ef migrations add <MigrationName> --startup-project  TodoApp.Web/TodoApp.Web.csproj --project TodoApp.Infrastructure/TodoApp.Infrastructure.csproj

# Update/Seed database from within the Web project
cd TodoApp.Web
dotnet ef database update --context ApplicationDbContext --project ../TodoApp.Infrastructure/TodoApp.Infrastructure.csproj --startup-project TodoApp.Web.csproj

```

# Run the project

```bash
# Update/Seed database from within the Web project
cd TodoApp.Web
dotnet ef database update --context ApplicationDbContext --project ../TodoApp.Infrastructure/TodoApp.Infrastructure.csproj --startup-project TodoApp.Web.csproj

./start.sh
```

Open the swagger page:

[http://localhost:5131/swagger](http://localhost:5131/swagger)


## Security

Development secret key generation for development purposes:
```bash
# Command used 
dd if=/dev/urandom bs=32 count=1 | base64
```