using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        
        private readonly LotteryDbContext _lotteryContext;

        public UserRepository(LotteryDbContext lotteryContext)
        {
            _lotteryContext = lotteryContext;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return await _lotteryContext.Users.ToListAsync();
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            return await _lotteryContext.Users
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return await _lotteryContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {


            _lotteryContext.Users.Add(user);
            await _lotteryContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel?> UpdateUserAsync(UserModel user)
        {
            var existing = await _lotteryContext.Users.FindAsync(user.Id);
            if (existing == null) return null;

            _lotteryContext.Entry(existing).CurrentValues.SetValues(user);


            await _lotteryContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _lotteryContext.Users.FindAsync(id);
            if (user == null) return false;

            _lotteryContext.Users.Remove(user);
            await _lotteryContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsUserAsync(int id)
        {
            return await _lotteryContext.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExistsUserAsync(string email)
        {
            return await _lotteryContext.Users.AnyAsync(u => u.Email == email);
        }

    }
}
