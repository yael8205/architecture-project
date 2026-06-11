using LotteryApi.Models;
using MongoDB.Driver;
using System.Linq;

namespace LotteryApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserModel> _users;

        public UserRepository(IMongoCollection<UserModel> users)
        {
            _users = users;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<UserModel?> GetUserByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<UserModel?> UpdateUserAsync(UserModel user)
        {
            var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return result.MatchedCount == 0 ? null : user;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var result = await _users.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistsUserAsync(string id)
        {
            return await _users.Find(u => u.Id == id).AnyAsync();
        }

        public async Task<bool> EmailExistsUserAsync(string email)
        {
            return await _users.Find(u => u.Email == email).AnyAsync();
        }
    }
}
