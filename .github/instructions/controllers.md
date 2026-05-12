---
applyTo:
  - "WebApiShop/Controllers/**/*.cs"
---

# API Controllers Guidelines

## Controller Structure & Inheritance
- **Always inherit from `ControllerBase`** (not Controller)
- Use `[Route("api/[controller]")]` and `[ApiController]` attributes
- Return `ActionResult<T>` for all endpoints (never plain objects)

## Dependency Injection Pattern
```csharp
public class EntityController : ControllerBase
{
    private readonly IEntityService _service;
    private readonly ILogger<EntityController> _logger;
    private readonly IConfiguration _configuration; // If needed for JWT/config
    
    public EntityController(IEntityService service, ILogger<EntityController> logger)
    {
        _service = service;
        _logger = logger;
    }
}
```

## HTTP Methods & Status Codes

### GET Endpoints
```csharp
[HttpGet("{id}")]
public async Task<ActionResult<EntityDTO>> Get(int id)
{
    var result = await _service.GetByIdAsync(id);
    if (result == null) return NoContent(); // 204
    return Ok(result); // 200
}

[HttpGet]
public async Task<ActionResult<IEnumerable<EntityDTO>>> GetAll()
{
    var results = await _service.GetAllAsync();
    return Ok(results); // Always 200, even for empty collections
}
```

### POST Endpoints
```csharp
[HttpPost]
public async Task<ActionResult<EntityDTO>> Post([FromBody] CreateEntityDTO dto)
{
    var result = await _service.CreateAsync(dto);
    if (result == null) return BadRequest("Validation failed");
    
    return CreatedAtAction(nameof(Get), new { id = result.Id }, result); // 201
}
```

### PUT Endpoints
```csharp
[HttpPut("{id}")]
public async Task<IActionResult> Put(int id, [FromBody] UpdateEntityDTO dto)
{
    await _service.UpdateAsync(id, dto);
    return Ok(); // 200 or NoContent() for 204
}
```

### DELETE Endpoints
```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    var success = await _service.DeleteAsync(id);
    if (!success) return NotFound(); // 404
    return NoContent(); // 204
}
```

## Authorization Patterns

### Public Endpoints (No Auth Required)
```csharp
[HttpGet]
[AllowAnonymous] // Explicitly mark as public
public async Task<ActionResult<List<ProductDto>>> GetProducts()
```

### Authenticated Endpoints
```csharp
[HttpPost]
[Authorize] // Requires valid JWT token
public async Task<ActionResult> CreateOrder([FromBody] OrderDto order)
```

### Admin-Only Endpoints
```csharp
[HttpGet]
[Authorize(Roles = "Admin")] // Only admin users
public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAllUsers()
```

### Controller-Level Authorization
```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize] // All endpoints require auth by default
public class OrdersController : ControllerBase
{
    // Individual endpoints can override with [AllowAnonymous]
}
```

## Authentication-Specific Patterns

### JWT Cookie Management (UsersController)
```csharp
[HttpPost("Login")]
[AllowAnonymous]
public async Task<ActionResult> Login([FromBody] LoginDTO loginDto)
{
    string? token = await _userService.LoginAsync(loginDto);
    if (token == null)
    {
        _logger.LogInformation("Login failed: UserEmail={UserEmail}", loginDto.UserEmail);
        return Unauthorized("Invalid credentials");
    }
    
    _logger.LogInformation("Login success: UserEmail={UserEmail}", loginDto.UserEmail);
    SetTokenCookie(token);
    return Ok(new { message = "Login successful" });
}

private void SetTokenCookie(string token)
{
    var expiresMinutes = double.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");
    var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    
    Response.Cookies.Append("jwt", token, new CookieOptions
    {
        HttpOnly = true,
        Secure = !isDev, // Only HTTPS in production
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes)
    });
}
```

### Logout Pattern
```csharp
[HttpPost("Logout")]
[Authorize]
public IActionResult Logout()
{
    Response.Cookies.Delete("jwt");
    return Ok(new { message = "Logged out successfully" });
}
```

## Error Handling & Logging

### Logging Patterns
```csharp
// Success scenarios
_logger.LogInformation("Entity created: Id={Id}", result.Id);

// Failure scenarios  
_logger.LogWarning("Entity not found: Id={Id}", id);

// Authentication events
_logger.LogInformation("Login attempt: UserEmail={UserEmail}", loginDto.UserEmail);
```

### Error Response Patterns
```csharp
// Validation errors
if (!ModelState.IsValid)
    return BadRequest(ModelState);

// Business logic errors
if (result == null)
    return BadRequest("Business rule violation message");

// Not found
if (entity == null)
    return NotFound($"Entity with id {id} not found");

// Unauthorized
return Unauthorized("Access denied message");
```

## Query Parameters & Filtering
```csharp
[HttpGet]
public async Task<ActionResult<List<ProductDto>>> GetProducts(
    [FromQuery] int position = 0,
    [FromQuery] int skip = 0, 
    [FromQuery] int? productId = null,
    [FromQuery] string? name = null,
    [FromQuery] float? minPrice = null,
    [FromQuery] float? maxPrice = null,
    [FromQuery] int[]? categoryIds = null,
    [FromQuery] string? description = null)
{
    var products = await _productService.GetProductsAsync(
        position, skip, productId, name, minPrice, maxPrice, categoryIds, description);
    
    if (products == null || !products.Any()) 
        return NoContent();
        
    return Ok(products);
}
```

## Controller Responsibilities (What NOT to do)

### ❌ Don't Put Business Logic in Controllers
```csharp
// BAD - Business logic in controller
[HttpPost]
public async Task<ActionResult> CreateUser([FromBody] UserDto userDto)
{
    // Don't do validation here
    if (string.IsNullOrEmpty(userDto.Email)) return BadRequest();
    
    // Don't do password hashing here
    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
    
    // Don't do direct database access
    var user = new User { Email = userDto.Email, Password = hashedPassword };
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
}
```

### ✅ Keep Controllers Thin
```csharp
// GOOD - Delegate to service layer
[HttpPost]
public async Task<ActionResult> CreateUser([FromBody] UserDto userDto)
{
    var result = await _userService.CreateUserAsync(userDto);
    if (result == null) return BadRequest("User creation failed");
    
    return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
}
```

## Common Controller Patterns by Entity

### UsersController
- POST /api/users (Register) - [AllowAnonymous]
- POST /api/users/login - [AllowAnonymous] 
- GET /api/users/{id} - [Authorize]
- GET /api/users - [Authorize(Roles = "Admin")]
- PUT /api/users/{id} - [Authorize]
- POST /api/users/logout - [Authorize]

### ProductsController  
- GET /api/products - [AllowAnonymous] (with filtering)
- POST /api/products/rate - [Authorize]

### CategoriesController
- GET /api/categories - [AllowAnonymous]

### OrdersController
- GET /api/orders/{id} - [Authorize] 
- POST /api/orders - [Authorize]

Remember: Controllers are the **orchestration layer** - they coordinate between the HTTP request/response and the business logic, but contain minimal logic themselves.