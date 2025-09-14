FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CartService/CartService.sln ./CartService/
COPY CartService/CartService.API/CartService.API.csproj ./CartService/CartService.API/
COPY CartService/CartService.Application/CartService.Application.csproj ./CartService/CartService.Application/
COPY CartService/CartService.Domain/CartService.Domain.csproj ./CartService/CartService.Domain/
COPY CartService/CartService.Infrastructure/CartService.Infrastructure.csproj ./CartService/CartService.Infrastructure/
COPY Shared/Shared.Contracts/Shared.Contracts.csproj ./Shared/Shared.Contracts/

WORKDIR /src/CartService
RUN dotnet restore

WORKDIR /src
COPY CartService/ ./CartService/
COPY Shared/ ./Shared/

WORKDIR /src/CartService/CartService.API
RUN dotnet publish CartService.API.csproj -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "CartService.API.dll"]