# Repository & Data Access Rules

## Repository Pattern Overview

**Purpose**: Centralize all database operations, decouple EF Core from business logic layers.

### Generic Repository Interface
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task SaveAsync();
}
```

---

## EF Core Query Rules

### Rule 1: Always Use Async Methods
```csharp
// ✓ CORRECT
var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

// ❌ WRONG
var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
```

### Rule 2: Explicit Eager Loading
- **Use `.Include()`** for related entities when needed
- **Document why** each include is necessary
- **Avoid N+1 queries**: Load all required data in one query

```csharp
// ✓ CORRECT: Load with related entities explicitly
var order = await _dbContext.Orders
    .Include(o => o.Donor)
    .Include(o => o.Items)
    .FirstOrDefaultAsync(o => o.Id == orderId);

// ❌ WRONG: Lazy loading causes multiple queries
var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
var donor = order.Donor;  // Separate query (or error if lazy loading disabled)
```

### Rule 3: Project to DTOs Early
- **Use `.Select()` to map to DTO** before returning from Repository
- **Reduces data transfer** and avoids exposing internal entities

```csharp
// ✓ CORRECT: Project to DTO
public async Task<UserResponseDto> GetUserByIdAsync(int id)
{
    return await _dbContext.Users
        .Where(u => u.Id == id)
        .Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        })
        .FirstOrDefaultAsync();
}

// ❌ WRONG: Return entity directly
public async Task<User> GetUserByIdAsync(int id)
{
    return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
}
```

### Rule 4: Filter in Database, Not in Memory
```csharp
// ✓ CORRECT: Filter in query
var activeUsers = await _dbContext.Users
    .Where(u => u.IsActive)
    .ToListAsync();

// ❌ WRONG: Fetch all, then filter in memory
var users = await _dbContext.Users.ToListAsync();
var activeUsers = users.Where(u => u.IsActive).ToList();
```

---

## DTO Mapping Standards

### Mapping Rules
- **DTOs map to single entities** or aggregates, never cross-domain
- **Request DTOs** contain only writable fields
- **Response DTOs** contain read-only data (exclude sensitive fields)
- **Use AutoMapper or manual mapping** (be explicit, avoid magic)

### DTO Naming Convention
```
Create{Entity}Dto    → For POST/insertion (nullable properties for optional fields)
Update{Entity}Dto    → For PUT/PATCH (similar to Create, may exclude ID)
{Entity}ResponseDto  → For GET responses
{Entity}ListItemDto  → For list endpoints (minimal fields)
```

### DTO Implementation Example
```csharp
// REQUEST DTO
public class CreateUserDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}

// RESPONSE DTO (sensitive data excluded)
public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    // Password, PasswordHash NOT included
}

// LIST DTO (minimal fields)
public class UserListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

---

## Repository Implementation Patterns

### Generic Base Repository
```csharp
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LotteryDbContext _context;

    public Repository(LotteryDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id)
            ?? throw new NotFoundException($"{typeof(T).Name} not found.");
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
```

### Domain-Specific Repository (extends generic)
```csharp
public interface IUserRepository : IRepository<User>
{
    Task<UserResponseDto> GetUserWithOrdersAsync(int userId);
    Task<bool> EmailExistsAsync(string email);
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(LotteryDbContext context) : base(context) { }

    public async Task<UserResponseDto> GetUserWithOrdersAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Orders)
            .Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Orders = u.Orders.Count
            })
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("User not found.");
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
```

---

## Async Patterns

### Rule 1: All I/O is Async
```csharp
// ✓ CORRECT
public async Task<List<Order>> GetOrdersByUserAsync(int userId)
{
    return await _dbContext.Orders
        .Where(o => o.UserId == userId)
        .ToListAsync();
}

// ❌ WRONG: Synchronous blocking call
public List<Order> GetOrdersByUser(int userId)
{
    return _dbContext.Orders
        .Where(o => o.UserId == userId)
        .ToList();
}
```

### Rule 2: Never Block on Async
```csharp
// ✓ CORRECT
public async Task ProcessOrderAsync(int orderId)
{
    var order = await _orderRepository.GetByIdAsync(orderId);
    // ... process
}

// ❌ WRONG: .Result causes deadlocks
public void ProcessOrder(int orderId)
{
    var order = _orderRepository.GetByIdAsync(orderId).Result;  // DEADLOCK!
}
```

### Rule 3: Propagate Async Up the Stack
```
Controller
    ↓ await
Service
    ↓ await
Repository (Database call - async)
```

---

## Transactions & Unit of Work

### Multi-Step Operations
```csharp
public async Task<Order> CreateOrderWithItemsAsync(CreateOrderDto dto)
{
    using (var transaction = await _context.Database.BeginTransactionAsync())
    {
        try
        {
            var order = new Order { /* ... */ };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in dto.Items)
            {
                var orderItem = new OrderItem { OrderId = order.Id, /* ... */ };
                _context.OrderItems.Add(orderItem);
            }
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return order;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```

---

## Exception Handling in Repositories

```csharp
public async Task<User> GetByIdAsync(int id)
{
    try
    {
        return await _context.Users.FindAsync(id)
            ?? throw new NotFoundException($"User with ID {id} not found.");
    }
    catch (DbUpdateException ex)
    {
        _logger.LogError($"Database error: {ex.Message}");
        throw new InvalidOperationException("Failed to retrieve user.", ex);
    }
}
```

---

## Performance Considerations

### Pagination
```csharp
public async Task<PaginatedResult<T>> GetPagedAsync(int pageNumber, int pageSize)
{
    var total = await _context.Set<T>().CountAsync();
    var items = await _context.Set<T>()
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new PaginatedResult<T>
    {
        Items = items,
        TotalCount = total,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
}
```

### Avoid Unnecessary Data Transfer
```csharp
// ✓ Only select needed columns
var data = await _context.Users
    .Select(u => new { u.Id, u.Name })
    .ToListAsync();

// ❌ Load entire entity
var data = await _context.Users.ToListAsync();
```
