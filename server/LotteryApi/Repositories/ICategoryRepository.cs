using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryModel> CreateCategoryAsync(CategoryModel Category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
        Task<CategoryModel?> GetCategoryByIdAsync(int id);
        Task<CategoryModel?> UpdateCategoryAsync(CategoryModel category);
    }
}