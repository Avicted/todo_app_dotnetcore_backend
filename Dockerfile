# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Install EF Core CLI tools
RUN dotnet tool install --global dotnet-ef

# Add the .NET tools to the PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the project files and restore dependencies
COPY ["TodoApp.Web/TodoApp.Web.csproj", "TodoApp.Web/"]
COPY ["TodoApp.Infrastructure/TodoApp.Infrastructure.csproj", "TodoApp.Infrastructure/"]

# Restore NuGet packages
RUN dotnet restore TodoApp.Web/TodoApp.Web.csproj

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build TodoApp.Web/TodoApp.Web.csproj -c Debug -o /app/build

# Publish the application
RUN dotnet publish TodoApp.Web/TodoApp.Web.csproj -c Debug -o /app/publish

# Use the official .NET 8 ASP.NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the runtime container
WORKDIR /app

# Copy the published application from the build container
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 1337

# Run the dotnet application
CMD ["dotnet", "TodoApp.Web.dll"]
