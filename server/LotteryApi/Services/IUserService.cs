using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IUserService
    {
        Task<AuthDto.LoginResponseDto?> AuthenticateAsync(string email, string password);
        Task<UserDto> CreateUserAsync(UserCreateDto usercreateDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto?> UpdateUserAsync(int id, UserUpdateDto userupdateDto);
    }
}