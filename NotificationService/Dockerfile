FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY NotificationService.sln ./
COPY NotificationService/NotificationService.csproj ./NotificationService/
RUN dotnet restore
COPY . .
WORKDIR /src/NotificationService
RUN dotnet publish NotificationService.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "NotificationService.dll"]