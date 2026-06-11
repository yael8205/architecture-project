# Core Development Rules & Standards

## Naming Conventions

### C# / Server-Side
- **Classes**: PascalCase (`AuthService`, `DonorRepository`)
- **Methods**: PascalCase (`GetUserById`, `ValidateEmail`)
- **Private Fields**: camelCase with underscore prefix (`_logger`, `_dbContext`)
- **Constants**: UPPER_SNAKE_CASE (`MAX_RETRY_ATTEMPTS`, `DEFAULT_TIMEOUT`)
- **Interfaces**: PascalCase with `I` prefix (`IAuthService`, `IRepository<T>`)
- **DTOs**: Suffix with `Dto` (`CreateUserDto`, `UserResponseDto`)

### Angular / Client-Side
- **Components**: kebab-case files, PascalCase classes (`user-profile.component.ts` → `UserProfileComponent`)
- **Services**: kebab-case files, PascalCase classes (`auth.service.ts` → `AuthService`)
- **Models/Interfaces**: PascalCase (`User`, `AuthToken`, `ShoppingCart`)
- **Variables**: camelCase (`currentUser`, `isLoading`)
- **Constants**: UPPER_SNAKE_CASE (`API_BASE_URL`, `MAX_ITEMS_PER_PAGE`)

---

## No-Bypass Safety Protocols

### Rule 1: Mandatory Validation
- **All inputs** from clients must be validated before processing
- **No direct database queries** based on user input without sanitization
- **All DTOs** must implement data annotation validators
- **Backend must re-validate** even if frontend validates

### Rule 2: Authentication & Authorization Checks
- **Every endpoint** requires explicit `[Authorize]` or documented public access
- **No hardcoded credentials** or secrets in code
- **Use dependency injection** for `IAuthService` in controllers
- **Check user permissions** before data access, not after

### Rule 3: Error Handling & Logging
- **Never return raw exceptions** to clients
- **Log all errors** with context (user ID, operation, timestamp)
- **Return generic error messages** to clients; detailed logs server-side only
- **Use custom exception classes** (`NotFoundException`, `UnauthorizedException`)

### Rule 4: Data Access Isolation
- **All database operations** route through Repositories, never directly in Controllers
- **Services call Repositories**, Controllers call Services
- **No business logic** in Controllers (validation, calculation, transformation)
- **No direct DbContext** access outside Repository layer

### Rule 5: Async/Await Consistency
- **All I/O operations** must be async (`async/await`)
- **Never use `.Result` or `.Wait()`** (deadlock risk)
- **Propagate async** through all layers (Controller → Service → Repository)

---

## Code Structure Standards

### Layer Responsibility
```
Controllers    → Route requests, deserialize input, call Services
Services       → Business logic, orchestration, validation
Repositories   → Data access, EF Core queries, entity mapping
Models         → Domain entities (database representations)
DTOs           → Data transfer objects (API contracts)
Enums          → Shared constants/statuses
```

### File Organization
- **One class per file** (except closely related enums)
- **Logical grouping** in folders by domain (Auth, Donors, Gifts, Orders)
- **Shared utilities** in dedicated folders (`Utilities/`, `Helpers/`)

---

## Code Quality Standards

### Principle: DRY (Don't Repeat Yourself)
- Extract common patterns into base classes or helpers
- Reuse existing services instead of duplicating logic

### Principle: Single Responsibility
- Each class has one reason to change
- Avoid multi-purpose utilities; split into focused services

### Principle: Fail Fast
- Validate at entry points (Controllers/endpoints)
- Throw exceptions immediately on invalid state
- Don't allow invalid data to propagate through layers

### Principle: Keep It Simple
- Prefer readable code over clever code
- Comment complex logic; obvious code doesn't need comments
- Limit method length (aim for < 30 lines)

---

## Common Mistakes (Avoid)

❌ Passing raw user IDs without verification  
❌ Async methods without `await` keyword  
❌ Catching generic `Exception` without logging  
❌ Database queries inside Controllers  
❌ Hardcoded configuration values  
❌ Skipping input validation on "internal" endpoints  
❌ Using `.Result` instead of `await`  
❌ Returning `Ok()` with raw entity objects (use DTOs)  

---

## Documentation Requirements

- **Public APIs**: XML comments (`/// <summary>`)
- **Complex methods**: Explain WHY, not just WHAT
- **Breaking changes**: Document in PR description and commit message
