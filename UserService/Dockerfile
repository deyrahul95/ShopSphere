# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["UserService.sln", "./"]
COPY ["UserService.API/UserService.API.csproj", "UserService.API/"]
COPY ["UserService.Application/UserService.Application.csproj", "UserService.Application/"]
COPY ["UserService.Domain/UserService.Domain.csproj", "UserService.Domain/"]
COPY ["UserService.Infrastructure/UserService.Infrastructure.csproj", "UserService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "UserService.API/UserService.API.csproj"

# Copy all files and build
COPY . .
WORKDIR "/src/UserService.API"
RUN dotnet publish "UserService.API.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose Port
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "UserService.API.dll"]
