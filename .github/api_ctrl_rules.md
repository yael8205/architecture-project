# API Controller & REST Standards

## REST Principles

### HTTP Methods
- **GET**: Retrieve data (safe, idempotent)
- **POST**: Create new resources
- **PUT**: Replace entire resource
- **PATCH**: Partial update
- **DELETE**: Remove resource

### Status Codes
- **200 OK**: Successful GET, PUT, PATCH
- **201 Created**: Successful POST (include `Location` header)
- **204 No Content**: Successful DELETE, or successful POST/PUT with no response body
- **400 Bad Request**: Invalid input (validation failure)
- **401 Unauthorized**: Missing/invalid authentication
- **403 Forbidden**: Authenticated but lacks permission
- **404 Not Found**: Resource doesn't exist
- **409 Conflict**: Resource conflict (duplicate key, state error)
- **500 Internal Server Error**: Unexpected server error

---

## Controller Structure

### Base Pattern
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    // Endpoints here
}
```

### Routing Conventions
```csharp
// List/Get all
[HttpGet]
public async Task<IActionResult> GetAll()

// Get by ID
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)

// Create
[HttpPost]
public async Task<IActionResult> Create(CreateDto dto)

// Update (full replacement)
[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, UpdateDto dto)

// Update (partial)
[HttpPatch("{id}")]
public async Task<IActionResult> Patch(int id, JsonPatchDocument<UpdateDto> patch)

// Delete
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)

// Custom actions (use verbs)
[HttpPost("{id}/publish")]
public async Task<IActionResult> Publish(int id)
```

---

## Input Validation

### Rule 1: Validate All Input
```csharp
// ✓ CORRECT: Use model validation
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // Process...
}

// ❌ WRONG: Skip validation
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    // No validation, directly process
}
```

### Rule 2: DTO Data Annotations
```csharp
public class CreateUserDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Name { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required]
    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }
}
```

### Rule 3: Custom Validation Logic
```csharp
// In Service Layer, NOT Controller
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    try
    {
        var user = await _userService.CreateUserAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
    catch (DuplicateEmailException ex)
    {
        return Conflict(new { error = ex.Message });
    }
}
```

---

## Response Wrapping

### Standard Response Format
```csharp
// Generic response wrapper
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string> Errors { get; set; }
}

// Example responses
// Success (200)
{
    "success": true,
    "data": { "id": 1, "name": "John", "email": "john@example.com" },
    "message": "User retrieved successfully"
}

// Validation error (400)
{
    "success": false,
    "message": "Validation failed",
    "errors": {
        "email": "Invalid email format",
        "age": "Age must be between 18 and 120"
    }
}

// Not found (404)
{
    "success": false,
    "message": "User not found"
}
```

### Helper Methods
```csharp
public class ControllerBase
{
    protected IActionResult ApiSuccess<T>(T data, string message = null, int statusCode = 200)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message ?? "Operation successful"
        };
        return StatusCode(statusCode, response);
    }

    protected IActionResult ApiError(string message, Dictionary<string, string> errors = null, int statusCode = 400)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
        return StatusCode(statusCode, response);
    }
}

// Usage in Controller
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    if (!ModelState.IsValid)
        return ApiError("Validation failed", ModelState.Values.SelectMany(v => v.Errors).ToErrors());

    var user = await _userService.CreateUserAsync(dto);
    return ApiSuccess(user, "User created successfully", 201);
}
```

---

## Authorization & Authentication

### Rule 1: Explicit Authorization
```csharp
// ✓ CORRECT: Explicit [Authorize]
[HttpGet]
[Authorize]  // Authenticated users only
public async Task<IActionResult> GetMyProfile()
{
    var userId = GetCurrentUserId();
    var user = await _userService.GetUserAsync(userId);
    return ApiSuccess(user);
}

// For specific roles
[HttpDelete("{id}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Delete(int id)
{
    // Admin-only operation
}

// ❌ WRONG: Forgot to add [Authorize]
[HttpGet]
public async Task<IActionResult> GetUserData()  // Public! Security issue
{
    // Sensitive data returned
}
```

### Rule 2: Extract Current User
```csharp
private int GetCurrentUserId()
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        throw new UnauthorizedException("User ID not found in token.");
    
    return int.Parse(userIdClaim.Value);
}

private string GetCurrentUserRole()
{
    return User.FindFirst(ClaimTypes.Role)?.Value
        ?? throw new UnauthorizedException("Role not found in token.");
}
```

### Rule 3: Verify Resource Ownership
```csharp
[HttpPut("{id}")]
[Authorize]
public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateUserDto dto)
{
    var currentUserId = GetCurrentUserId();
    
    // Verify ownership (No-Bypass Protocol)
    if (id != currentUserId)
        return Forbid();  // 403

    var user = await _userService.UpdateUserAsync(id, dto);
    return ApiSuccess(user);
}
```

---

## Endpoint Implementation Examples

### List Endpoint (with pagination)
```csharp
[HttpGet]
[Authorize]
public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        return ApiError("Invalid pagination parameters.");

    var result = await _userService.GetPagedAsync(pageNumber, pageSize);
    return ApiSuccess(result);
}
```

### Get by ID Endpoint
```csharp
[HttpGet("{id}")]
[Authorize]
public async Task<IActionResult> GetById(int id)
{
    if (id <= 0)
        return ApiError("Invalid ID.", statusCode: 400);

    try
    {
        var user = await _userService.GetUserByIdAsync(id);
        return ApiSuccess(user);
    }
    catch (NotFoundException ex)
    {
        return ApiError(ex.Message, statusCode: 404);
    }
}
```

### Create Endpoint
```csharp
[HttpPost]
[Authorize]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    if (!ModelState.IsValid)
        return ApiError("Validation failed.", ToErrorDictionary(ModelState), 400);

    try
    {
        var user = await _userService.CreateUserAsync(dto);
        return ApiSuccess(user, "User created successfully.", 201);
    }
    catch (DuplicateEmailException ex)
    {
        return ApiError(ex.Message, statusCode: 409);
    }
}
```

### Update Endpoint
```csharp
[HttpPut("{id}")]
[Authorize]
public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
{
    if (id <= 0)
        return ApiError("Invalid ID.", statusCode: 400);

    if (!ModelState.IsValid)
        return ApiError("Validation failed.", ToErrorDictionary(ModelState), 400);

    var currentUserId = GetCurrentUserId();
    if (id != currentUserId && !User.IsInRole("Admin"))
        return Forbid();

    try
    {
        var user = await _userService.UpdateUserAsync(id, dto);
        return ApiSuccess(user, "User updated successfully.");
    }
    catch (NotFoundException ex)
    {
        return ApiError(ex.Message, statusCode: 404);
    }
}
```

### Delete Endpoint
```csharp
[HttpDelete("{id}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Delete(int id)
{
    if (id <= 0)
        return ApiError("Invalid ID.", statusCode: 400);

    try
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();  // 204
    }
    catch (NotFoundException ex)
    {
        return ApiError(ex.Message, statusCode: 404);
    }
}
```

---

## Error Handling in Controllers

### Centralized Error Handling (Global Middleware)
```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unhandled exception: {ex.Message}");
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "An unexpected error occurred."
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

// Register in Program.cs
app.UseMiddleware<ExceptionMiddleware>();
```

---

## Best Practices

### Rule 1: Keep Controllers Thin
- Business logic → Services
- Data access → Repositories
- Controllers → Orchestration only

### Rule 2: Consistent Error Responses
- Always return wrapped responses
- Include meaningful error messages for debugging
- Never expose stack traces to clients

### Rule 3: Use Proper HTTP Methods & Status Codes
- Idempotent operations use GET, PUT, DELETE
- State-changing operations use POST, PUT, PATCH
- Return appropriate status codes

### Rule 4: Validate Before Processing
- Reject invalid input early with 400 Bad Request
- Validate ownership/permissions before operations
- Let service layer throw custom exceptions

### Rule 5: Logging
```csharp
_logger.LogInformation($"User {userId} requested data.");
_logger.LogWarning($"Failed to retrieve resource: {ex.Message}");
_logger.LogError($"Database error: {ex}", ex);
```

---

## Common Mistakes (Avoid)

❌ Returning raw entities instead of DTOs  
❌ Missing [Authorize] on protected endpoints  
❌ Inconsistent response formats  
❌ Insufficient validation of input  
❌ Returning stack traces to clients  
❌ Direct database queries in Controller  
❌ Not checking resource ownership before modification  
❌ Using synchronous calls in async controller  
❌ Hardcoding HTTP status codes without constants  
❌ Not logging errors for debugging
