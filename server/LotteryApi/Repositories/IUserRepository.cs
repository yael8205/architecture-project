using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUserAsync(UserModel user);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> EmailExistsUserAsync(string email);
        Task<bool> ExistsUserAsync(string id);
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task<UserModel?> GetUserByIdAsync(string id);
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<UserModel?> UpdateUserAsync(UserModel user);
    }
}