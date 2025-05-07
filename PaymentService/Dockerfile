FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PaymentService/PaymentService.sln ./PaymentService/
COPY PaymentService/PaymentService.API/PaymentService.API.csproj ./PaymentService/PaymentService.API/
COPY PaymentService/PaymentService.Application/PaymentService.Application.csproj ./PaymentService/PaymentService.Application/
COPY PaymentService/PaymentService.Domain/PaymentService.Domain.csproj ./PaymentService/PaymentService.Domain/
COPY PaymentService/PaymentService.Infrastructure/PaymentService.Infrastructure.csproj ./PaymentService/PaymentService.Infrastructure/
COPY Shared/Shared.Contracts/Shared.Contracts.csproj ./Shared/Shared.Contracts/

WORKDIR /src/PaymentService
RUN dotnet restore

WORKDIR /src
COPY PaymentService/ ./PaymentService/
COPY Shared/ ./Shared/

WORKDIR /src/PaymentService/PaymentService.API
RUN dotnet publish PaymentService.API.csproj -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "PaymentService.API.dll"]