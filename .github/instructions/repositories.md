---
applyTo:
  - "Repository/**/*.cs"
---

# Repository Layer Guidelines

## Repository Pattern & Responsibilities
- **Data Access Only** - No business logic, validation, or caching
- **EF Core Operations** - Use Entity Framework Core for all database interactions
- **Async/Await** - All database operations must be asynchronous
- **Interface-Based** - Every repository must implement an interface

## Repository Structure Pattern
```csharp
public class EntityRepository : IEntityRepository
{
    private readonly WebApiShop216328971Context _context;
    
    public EntityRepository(WebApiShop216328971Context context)
    {
        _context = context;
    }
    
    // Async CRUD operations only
}
```

## Interface Definition Pattern
```csharp
public interface IEntityRepository
{
    Task<Entity?> GetByIdAsync(int id);
    Task<IEnumerable<Entity>> GetAllAsync();
    Task<Entity> CreateAsync(Entity entity);
    Task UpdateAsync(int id, EntityDto dto);
    Task<bool> DeleteAsync(int id);
    
    // Entity-specific query methods
    Task<Entity?> GetByEmailAsync(string email);
}
```

## CRUD Operations Patterns

### Read Operations
```csharp
// Single entity by ID
public async Task<User?> GetByIdAsync(int id)
{
    return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
}

// All entities
public async Task<IEnumerable<User>> GetAllAsync()
{
    return await _context.Users.ToListAsync();
}

// Custom queries
public async Task<User?> GetByEmailAsync(string email)
{
    return await _context.Users
        .FirstOrDefaultAsync(u => u.UserEmail == email);
}

// Complex filtering (Products example)
public async Task<List<Product>> GetProductsAsync(
    int position, int skip, int? productId, string? name, 
    float? minPrice, float? maxPrice, int[]? categoryIds, string? description)
{
    var query = _context.Products.AsQueryable();
    
    if (productId.HasValue)
        query = query.Where(p => p.Id == productId.Value);
        
    if (!string.IsNullOrEmpty(name))
        query = query.Where(p => p.Name.Contains(name));
        
    if (minPrice.HasValue)
        query = query.Where(p => p.Price >= minPrice.Value);
        
    if (maxPrice.HasValue)
        query = query.Where(p => p.Price <= maxPrice.Value);
        
    if (categoryIds != null && categoryIds.Length > 0)
        query = query.Where(p => categoryIds.Contains(p.CategoryId));
        
    if (!string.IsNullOrEmpty(description))
        query = query.Where(p => p.Description.Contains(description));
    
    return await query.Skip(skip).Take(position).ToListAsync();
}
```

### Create Operations
```csharp
public async Task<User> CreateAsync(User user)
{
    if (user == null)
        throw new ArgumentNullException(nameof(user), "User cannot be null");
    
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
    return user;
}
```

### Update Operations
```csharp
// Update using DTO pattern
public async Task UpdateAsync(int id, UserDto userDto)
{
    User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    
    if (user == null)
        return; // Or throw exception based on business requirements
    
    // Map DTO properties to entity
    user.UserEmail = userDto.UserName;
    user.FirstName = userDto.FirstName;
    user.LastName = userDto.LastName;
    user.Password = userDto.Password;
    
    await _context.SaveChangesAsync();
}

// Alternative: Update using entity
public async Task<User?> UpdateAsync(User updatedUser)
{
    var existingUser = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);
    
    if (existingUser == null)
        return null;
    
    _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
    await _context.SaveChangesAsync();
    return existingUser;
}
```

### Delete Operations
```csharp
public async Task<bool> DeleteAsync(int id)
{
    var entity = await _context.Users.FindAsync(id);
    if (entity == null)
        return false;
    
    _context.Users.Remove(entity);
    await _context.SaveChangesAsync();
    return true;
}
```

## Authentication-Specific Patterns

### Login Repository Method
```csharp
public async Task<User?> LoginAsync(LoginDTO loginDto)
{
    return await _context.Users.FirstOrDefaultAsync(
        x => x.UserEmail == loginDto.UserEmail && 
             x.Password == loginDto.Password);
}
```

### User Existence Checks
```csharp
public async Task<bool> ExistsByEmailAsync(string email)
{
    return await _context.Users.AnyAsync(u => u.UserEmail == email);
}
```

## Entity Relationships & Includes

### Loading Related Data
```csharp
// Include related entities when needed
public async Task<Order?> GetOrderWithItemsAsync(int orderId)
{
    return await _context.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .FirstOrDefaultAsync(o => o.Id == orderId);
}

// User with orders
public async Task<User?> GetUserWithOrdersAsync(int userId)
{
    return await _context.Users
        .Include(u => u.Orders)
        .FirstOrDefaultAsync(u => u.Id == userId);
}
```

## Error Handling & Null Safety

### Null Handling Patterns
```csharp
// Always check for null inputs
public async Task<User> CreateAsync(User? user)
{
    if (user == null)
        throw new ArgumentNullException(nameof(user), "User cannot be null");
    
    // Proceed with creation
}

// Return null for not found (let service layer handle)
public async Task<User?> GetByIdAsync(int id)
{
    return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    // Don't throw exceptions for not found - return null
}
```

### Exception Handling
```csharp
// Let EF Core exceptions bubble up to service layer
// Don't catch and handle database exceptions in repository
public async Task<User> CreateAsync(User user)
{
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync(); // Let SQL exceptions bubble up
    return user;
}
```

## Performance Considerations

### Async Best Practices
```csharp
// Always use async methods
public async Task<List<User>> GetActiveUsersAsync()
{
    return await _context.Users
        .Where(u => u.IsActive)
        .ToListAsync(); // Not .ToList()
}
```

### Query Optimization
```csharp
// Use AsNoTracking for read-only operations
public async Task<IEnumerable<User>> GetAllUsersReadOnlyAsync()
{
    return await _context.Users
        .AsNoTracking()
        .ToListAsync();
}

// Project to DTOs in database when possible
public async Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync()
{
    return await _context.Users
        .Select(u => new UserSummaryDto
        {
            Id = u.Id,
            Email = u.UserEmail,
            FullName = u.FirstName + " " + u.LastName
        })
        .ToListAsync();
}
```

## DbContext Usage Patterns

### Context Injection
```csharp
public class UserRepository : IUserRepository
{
    private readonly WebApiShop216328971Context _context;
    
    public UserRepository(WebApiShop216328971Context context)
    {
        _context = context;
    }
    
    // Don't dispose context - DI container handles it
}
```

### Transaction Handling
```csharp
// For complex operations requiring transactions
public async Task<bool> TransferOrderAsync(int fromUserId, int toUserId, int orderId)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null || order.UserId != fromUserId)
            return false;
        
        order.UserId = toUserId;
        await _context.SaveChangesAsync();
        
        await transaction.CommitAsync();
        return true;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

## What NOT to Include in Repositories

### ❌ Business Logic
```csharp
// BAD - Don't validate business rules
public async Task<User> CreateUserAsync(User user)
{
    // Don't do this in repository
    if (user.Age < 18)
        throw new InvalidOperationException("User must be 18+");
    
    // Don't hash passwords here
    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
}
```

### ❌ Caching
```csharp
// BAD - Don't handle caching in repository
public async Task<User?> GetUserByIdAsync(int id)
{
    // Don't check cache here - that's service layer responsibility
    var cached = await _redis.GetAsync($"user:{id}");
    if (cached != null) return JsonSerializer.Deserialize<User>(cached);
}
```

### ❌ DTO Mapping
```csharp
// BAD - Don't map to DTOs in repository
public async Task<UserDto> GetUserDtoAsync(int id)
{
    var user = await _context.Users.FindAsync(id);
    return _mapper.Map<UserDto>(user); // This belongs in service layer
}
```

## Repository-Specific Patterns by Entity

### UserRepository
- GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync
- LoginAsync(LoginDTO) - returns User?
- GetByEmailAsync(string) - for uniqueness checks

### ProductRepository  
- GetProductsAsync with complex filtering
- GetByCategoryAsync(int categoryId)
- SearchAsync(string searchTerm)

### OrderRepository
- GetByUserIdAsync(int userId)
- GetOrderWithItemsAsync(int orderId) - with Include
- GetOrdersByDateRangeAsync(DateTime from, DateTime to)

### CategoryRepository
- GetAllAsync - simple read-only operations
- GetWithProductsAsync(int categoryId)

Remember: Repositories are the **data access boundary** - they translate between your domain entities and the database, nothing more.