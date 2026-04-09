using LotteryApi.Dtos;
using LotteryApi.Exceptions;
using LotteryApi.Models;
using LotteryApi.Repositories;
using static LotteryApi.Dtos.AuthDto;

namespace LotteryApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        //private readonly ILogger<UserService> _logger;

        public UserService(
           IUserRepository userRepository,
            ITokenService tokenService,
            IConfiguration configuration)
        // ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _configuration = configuration;
            //_logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return users.Select(MapToResponseDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? MapToResponseDto(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto usercreateDto)
        {
            if (await _userRepository.EmailExistsUserAsync(usercreateDto.Email))
            {
                throw new ConflictException($"כתובת האימייל {usercreateDto.Email} כבר רשומה במערכת.");
            }

            var user = new UserModel
            {
                Name = usercreateDto.Name,
                Email = usercreateDto.Email,
                Password = HashPassword(usercreateDto.Password),
                Phone = usercreateDto.Phone,
                Address = usercreateDto.Address,

            };

            var createdUser = await _userRepository.CreateUserAsync(user);
            if (createdUser == null)
            {
                throw new Exception("חלה שגיאה ביצירת המשתמש בבסיס הנתונים.");
            }

            return createdUser != null ? MapToResponseDto(createdUser) : null;
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UserUpdateDto userupdateDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null) return null;

            if (userupdateDto.Email != null && userupdateDto.Email != existingUser.Email)
            {
                if (await _userRepository.EmailExistsUserAsync(userupdateDto.Email))
                {
                    throw new ArgumentException($"Email {userupdateDto.Email} is already registered.");
                }
                existingUser.Email = userupdateDto.Email;
            }

            if (userupdateDto.Name != null) existingUser.Name = userupdateDto.Name;
            if (userupdateDto.Email != null) existingUser.Email = userupdateDto.Email;
            if (userupdateDto.Phone != null) existingUser.Phone = userupdateDto.Phone;
            if (userupdateDto.Address != null) existingUser.Address = userupdateDto.Address;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            return updatedUser != null ? MapToResponseDto(updatedUser) : null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<LoginResponseDto?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new BadRequestException("חובה למלא את כל השדות.");
            }
            var user = await _userRepository.GetUserByEmailAsync(email);

          


            var hashedPassword = HashPassword(password);
            if (user == null || user.Password != HashPassword(password))
            {
                // זריקת השגיאה תפעיל אוטומטית את ה-Middleware
                throw new UnauthorizedException("שם משתמש או סיסמה שגויים");
            }

            var token = _tokenService.GenerateToken(user.Id, user.Email, user.Name, user.Role, user.OrganizationId);
            var expiryMinutes = _configuration.GetValue<int>("JwtSettings:ExpiryMinutes", 60);

            //_logger.LogInformation("User {UserId} authenticated successfully", user.Id);

            return new LoginResponseDto
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = expiryMinutes * 60, // Convert to seconds
                User = MapToResponseDto(user)
            };
        }

        private static UserDto MapToResponseDto(UserModel user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                Orders = user.Orders?.Select(o => new OrderDto
                {
                    Id = o.Id,
                    ParticipantId= o.ParticipantId,
                    ParticipantName = user.Name,
                    SumPrice = o.SumPrice,
                    date= o.date,
                    PackagesInOrder = o.PackagesInOrder?.Select(p => new PackageInOrderDto
                    {
                        PackageId = p.PackageId,
                        PackageName = p.Package?.Name,
                        PriceAtPurchase = p.PriceAtPurchase,

                        GiftsInPackage = p.GiftsInPackage?.Select(g => new GiftInOrderDto
                        {
                            Id = g.Id,
                            GiftId = g.GiftId,
                            GiftName = g.Gift?.Name,
                            GiftPictureUrl = g.Gift?.PictureUrl,
                            GiftCardPrice = g.Gift?.CardPrice.ToString(),
                            IsWinner = g.IsWinner
                        }).ToList() ??[ ]
                    }).ToList() ?? []
                }).ToList() ?? []
            };
        }

        // Simplified password hashing - In production, use BCrypt, Argon2, or Identity framework
        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
