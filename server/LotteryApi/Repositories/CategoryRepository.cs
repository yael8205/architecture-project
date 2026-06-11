using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<CategoryModel> _categories;

        public CategoryRepository(IMongoCollection<CategoryModel> categories)
        {
            _categories = categories;
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }

        public async Task<CategoryModel?> GetCategoryByIdAsync(string id)
        {
            return await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CategoryModel> CreateCategoryAsync(CategoryModel Category)
        {
            await _categories.InsertOneAsync(Category);
            return Category;
        }

        public async Task<CategoryModel?> UpdateCategoryAsync(CategoryModel category)
        {
            var result = await _categories.ReplaceOneAsync(c => c.Id == category.Id, category);
            return result.MatchedCount == 0 ? null : category;
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var result = await _categories.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
