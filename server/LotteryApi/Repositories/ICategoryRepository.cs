using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryModel> CreateCategoryAsync(CategoryModel Category);
        Task<bool> DeleteCategoryAsync(string id);
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
        Task<CategoryModel?> GetCategoryByIdAsync(string id);
        Task<CategoryModel?> UpdateCategoryAsync(CategoryModel category);
    }
}