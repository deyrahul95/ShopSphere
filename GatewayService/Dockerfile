FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY GatewayService.sln ./
COPY GatewayService.Api/GatewayService.Api.csproj ./GatewayService.Api/

RUN dotnet restore
COPY . .

WORKDIR /src/GatewayService.Api
RUN dotnet publish GatewayService.Api.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "GatewayService.Api.dll"]