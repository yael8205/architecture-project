using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryUpdateDto updateCategory);
    }
}