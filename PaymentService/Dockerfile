FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY PaymentService.sln ./
COPY PaymentService.API/PaymentService.API.csproj ./PaymentService.API/
COPY PaymentService.Application/PaymentService.Application.csproj ./PaymentService.Application/
COPY PaymentService.Domain/PaymentService.Domain.csproj ./PaymentService.Domain/
COPY PaymentService.Infrastructure/PaymentService.Infrastructure.csproj ./PaymentService.Infrastructure/
RUN dotnet restore
COPY . .
WORKDIR /src/PaymentService.API
RUN dotnet publish PaymentService.API.csproj -c Release -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "PaymentService.API.dll"]