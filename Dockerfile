# WebApiShop Dockerfile
# Multi-stage build for .NET 9 Web API

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY WebApiShop.sln ./

# Copy project files for dependency restoration
COPY WebApiShop/WebApiShop.csproj WebApiShop/
COPY Services/Services.csproj Services/
COPY Repository/Repository.csproj Repository/
COPY Entities/Entities.csproj Entities/
COPY DTOs/DTOs.csproj DTOs/
COPY Tests/Tests.csproj Tests/

# Restore dependencies
RUN dotnet restore WebApiShop.sln

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/WebApiShop
RUN dotnet build WebApiShop.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish WebApiShop.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published application
COPY --from=publish /app/publish .

# Create directory for logs (NLog configuration)
USER root
RUN mkdir -p /app/logs && chown -R appuser /app/logs
USER appuser

# Expose port
EXPOSE 8080

# Environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "WebApiShop.dll"]