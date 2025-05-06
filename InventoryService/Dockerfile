FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY InventoryService.sln ./
COPY InventoryService.API/InventoryService.API.csproj ./InventoryService.API/
COPY InventoryService.Application/InventoryService.Application.csproj ./InventoryService.Application/
COPY InventoryService.Domain/InventoryService.Domain.csproj ./InventoryService.Domain/
COPY InventoryService.Infrastructure/InventoryService.Infrastructure.csproj ./InventoryService.Infrastructure/
RUN dotnet restore
COPY . .
WORKDIR /src/InventoryService.API
RUN dotnet publish InventoryService.API.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "InventoryService.API.dll"]