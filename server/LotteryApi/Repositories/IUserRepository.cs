using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUserAsync(UserModel user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> EmailExistsUserAsync(string email);
        Task<bool> ExistsUserAsync(int id);
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<UserModel?> UpdateUserAsync(UserModel user);
    }
}