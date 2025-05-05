FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY OrderService.sln ./
COPY OrderService.API/OrderService.API.csproj ./OrderService.API/
COPY OrderService.Application/OrderService.Application.csproj ./OrderService.Application/
COPY OrderService.Domain/OrderService.Domain.csproj ./OrderService.Domain/
COPY OrderService.Infrastructure/OrderService.Infrastructure.csproj ./OrderService.Infrastructure/
RUN dotnet restore
COPY . .
WORKDIR /src/OrderService.API
RUN dotnet publish OrderService.API.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "OrderService.API.dll"]