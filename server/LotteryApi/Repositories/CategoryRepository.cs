using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly LotteryDbContext _lotteryContext;
        public CategoryRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }
        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            return await _lotteryContext.Categories.Include(c => c.Gifts).ToListAsync();
        }
        public async Task<CategoryModel?> GetCategoryByIdAsync(int id)
        {
            return await _lotteryContext.Categories
                .Include(c => c.Gifts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<CategoryModel> CreateCategoryAsync(CategoryModel Category)
        {
            _lotteryContext.Categories.Add(Category);
            await _lotteryContext.SaveChangesAsync();
            return Category;
        }
        public async Task<CategoryModel?> UpdateCategoryAsync(CategoryModel category)
        {
            var existing = await _lotteryContext.Categories.FindAsync(category.Id);
            if (existing == null)
                return null;
            _lotteryContext.Entry(existing).CurrentValues.SetValues(category);
            await _lotteryContext.SaveChangesAsync();
            return existing;

        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var existing = await _lotteryContext.Categories.FindAsync(id);
            if (existing == null)
                return false;
            _lotteryContext.Categories.Remove(existing);
            await _lotteryContext.SaveChangesAsync();
            return true;
        }

    }
}
