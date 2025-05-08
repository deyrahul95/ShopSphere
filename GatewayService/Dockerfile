FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY GatewayService.sln ./
COPY GatewayService.API/GatewayService.API.csproj ./GatewayService.API/

RUN dotnet restore
COPY . .

WORKDIR /src/GatewayService.API
RUN dotnet publish GatewayService.API.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "GatewayService.API.dll"]