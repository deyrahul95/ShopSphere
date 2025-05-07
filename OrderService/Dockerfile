FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY OrderService/OrderService.sln ./OrderService/
COPY OrderService/OrderService.API/OrderService.API.csproj ./OrderService/OrderService.API/
COPY OrderService/OrderService.Application/OrderService.Application.csproj ./OrderService/OrderService.Application/
COPY OrderService/OrderService.Domain/OrderService.Domain.csproj ./OrderService/OrderService.Domain/
COPY OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj ./OrderService/OrderService.Infrastructure/
COPY Shared/Shared.Contracts/Shared.Contracts.csproj ./Shared/Shared.Contracts/

WORKDIR /src/OrderService
RUN dotnet restore

WORKDIR /src
COPY OrderService/ ./OrderService/
COPY Shared/ ./Shared/

WORKDIR /src/OrderService/OrderService.API
RUN dotnet publish OrderService.API.csproj -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "OrderService.API.dll"]