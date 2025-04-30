FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ProductService.sln ./
COPY ProductService.API/ProductService.API.csproj ./ProductService.API/
COPY ProductService.Application/ProductService.Application.csproj ./ProductService.Application/
COPY ProductService.Domain/ProductService.Domain.csproj ./ProductService.Domain/
COPY ProductService.Infrastructure/ProductService.Infrastructure.csproj ./ProductService.Infrastructure/
RUN dotnet restore
COPY . .
WORKDIR /src/ProductService.API
RUN dotnet publish ProductService.API.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "ProductService.API.dll"]