FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NotificationService/NotificationService.sln ./NotificationService/
COPY NotificationService/NotificationService/NotificationService.csproj ./NotificationService/NotificationService/
COPY Shared/Shared.Contracts/Shared.Contracts.csproj ./Shared/Shared.Contracts/

WORKDIR /src/NotificationService
RUN dotnet restore

WORKDIR /src
COPY NotificationService/ ./NotificationService/
COPY Shared/ ./Shared/

WORKDIR /src/NotificationService/NotificationService
RUN dotnet publish NotificationService.csproj -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "NotificationService.dll"]