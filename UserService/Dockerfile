FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY UserService.sln ./
COPY UserService.API/UserService.API.csproj ./UserService.API/
COPY UserService.Application/UserService.Application.csproj ./UserService.Application/
COPY UserService.Domain/UserService.Domain.csproj ./UserService.Domain/
COPY UserService.Infrastructure/UserService.Infrastructure.csproj ./UserService.Infrastructure/
RUN dotnet restore
COPY . .
WORKDIR /src/UserService.API
RUN dotnet publish UserService.API.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "UserService.API.dll"]
