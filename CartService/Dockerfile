FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY CartService.sln ./
COPY CartService.API/CartService.API.csproj ./CartService.API/
COPY CartService.Application/CartService.Application.csproj ./CartService.Application/
COPY CartService.Domain/CartService.Domain.csproj ./CartService.Domain/
COPY CartService.Infrastructure/CartService.Infrastructure.csproj ./CartService.Infrastructure/
RUN dotnet restore
COPY . .
WORKDIR /src/CartService.API
RUN dotnet publish CartService.API.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "CartService.API.dll"]