# WebApiShop - AI Coding Agent Instructions

## Project Overview
WebApiShop is a professional RESTful API built with .NET 9 and C#, following **Clean Architecture** principles. This is an e-commerce shop API that manages users, products, categories, and orders with JWT authentication, Redis caching, and comprehensive logging.

## Architecture & Project Structure

### Clean Architecture Layers
```
WebApiShop/           # 🎯 API Layer (Controllers, Middleware)
├── Controllers/      # API endpoints, inherit from ControllerBase
├── Middlewares/      # Custom middleware (ErrorHandling, Rating)
└── Program.cs        # DI configuration, JWT setup, pipeline

Services/             # 🧠 Business Logic Layer
├── I*Service.cs      # Service interfaces
├── *Service.cs       # Business logic implementation
└── AutoMapper.cs     # Entity ↔ DTO mapping profiles

Repository/           # 💾 Data Access Layer
├── I*Repository.cs   # Repository interfaces
├── *Repository.cs    # EF Core implementations
├── Migrations/       # EF Core database migrations
└── *Context.cs       # DbContext

Entities/             # 📊 Database Models (EF Core)
└── *.cs              # Database entities (User, Product, Order, etc.)

DTOs/                 # 📦 Data Transfer Objects
└── *.cs              # Record-based DTOs for API communication

Tests/                # 🧪 Testing Suite
├── *UnitTest.cs      # Unit tests with Moq
└── *IntegrationTests.cs # Integration tests with TestBase
```

## Tech Stack & Dependencies

### Core Technologies
- **.NET 9** - Latest framework
- **EF Core 9** - ORM with SQL Server
- **AutoMapper** - Entity/DTO mapping
- **JWT Bearer Authentication** - Token-based auth with HttpOnly cookies
- **Redis (StackExchange.Redis)** - Caching layer
- **NLog** - Structured logging with email alerts
- **Swagger/OpenAPI** - API documentation with JWT support
- **xUnit + Moq** - Testing framework

### Key NuGet Packages
```xml
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.EntityFrameworkCore.SqlServer
AutoMapper.Extensions.Microsoft.DependencyInjection
StackExchange.Redis
NLog.Web.AspNetCore
Swashbuckle.AspNetCore
zxcvbn-core (password strength)
```

## Coding Standards & Patterns

### 1. Clean Architecture Principles
- **Controllers**: Thin layer, only orchestration. Call Services, return ActionResult<T>
- **Services**: Business logic, validation, JWT generation. Use async/await
- **Repositories**: Data access only. Use EF Core with async methods
- **DTOs**: Use C# records for immutability

### 2. Dependency Injection Pattern
- All layers connected via DI (configured in Program.cs)
- Use interfaces for all services and repositories
- Scoped lifetime for most services

### 3. Authentication & Authorization
- JWT tokens stored as HttpOnly cookies (not Authorization header)
- Role-based authorization: "Admin" vs "User" (determined by email prefix "admin")
- Use `[Authorize]`, `[AllowAnonymous]`, `[Authorize(Roles = "Admin")]`

### 4. Caching Strategy
- Redis for user data with 5-minute TTL
- Cache invalidation on updates
- Pattern: `user:{id}` for keys

### 5. Error Handling & Logging
- Global error handling via ErrorHandlingMiddleware
- NLog with JSON format, file + email targets
- Log authentication events (login success/failure)

## Build & Development Commands

### Essential Commands
```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run API (from root)
dotnet run --project WebApiShop

# Run tests
dotnet test

# EF Core migrations
dotnet ef migrations add MigrationName --project Repository
dotnet ef database update --project Repository
```

### Development Environment
- **Database**: SQL Server with connection string in appsettings.json
- **Redis**: localhost:6379 with password
- **Swagger**: Available in Development mode at /swagger
- **Logging**: Files written to c:\temp\, emails on errors

## Configuration Files

### appsettings.json Structure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "SQL Server connection",
    "Redis": "Redis connection with password"
  },
  "Jwt": {
    "Key": "Secret key for JWT signing",
    "Issuer": "WebApiShop",
    "Audience": "WebApiShopUsers",
    "ExpiresInMinutes": 60
  },
  "RedisOptions": {
    "Configuration": "Redis config",
    "DefaultTTLInMinutes": 5
  }
}
```

## Common Patterns & Best Practices

### API Endpoints Pattern
```csharp
[Route("api/[controller]")]
[ApiController]
public class EntityController : ControllerBase
{
    private readonly IEntityService _service;
    private readonly ILogger<EntityController> _logger;
    
    // GET, POST, PUT, DELETE with proper HTTP status codes
    // Use ActionResult<T> for all endpoints
}
```

### Service Layer Pattern
```csharp
public class EntityService : IEntityService
{
    private readonly IEntityRepository _repository;
    private readonly IMapper _mapper;
    private readonly IDatabase _redis;
    
    // Business logic with validation
    // Async methods, proper error handling
    // Cache management
}
```

### Repository Pattern
```csharp
public class EntityRepository : IEntityRepository
{
    private readonly DbContext _context;
    
    // EF Core operations
    // Async methods with proper null handling
    // No business logic - data access only
}
```

## Security Considerations
- Passwords validated with zxcvbn (minimum strength 3)
- JWT tokens in HttpOnly cookies (CSRF protection)
- SQL injection prevention via EF Core parameterized queries
- Input validation via Data Annotations

## Testing Strategy
- Unit tests for business logic (Services layer)
- Integration tests for full API flow
- Use Moq for mocking dependencies
- TestBase class for common test setup

---

**Remember**: This is a learning project following enterprise-grade patterns. Maintain clean separation of concerns, use async/await consistently, and follow the established architectural patterns.