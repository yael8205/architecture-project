using LotteryApi.Dtos;
using LotteryApi.Models;
using LotteryApi.Repositories;

namespace LotteryApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var category = await _categoryRepository.GetCategoriesAsync();
            return category.Select(d => new CategoryDto
            {
                Id = d.Id,
                Name = d.Name
            });
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id.ToString());
            return category != null ? new CategoryDto { Id = category.Id, Name = category.Name } : null;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto category)
        {
            var newCategory = new CategoryModel
            {
                Name = category.Name
            };

            var createCategory = await _categoryRepository.CreateCategoryAsync(newCategory);
            return new CategoryDto { Id = createCategory.Id, Name = createCategory.Name };
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryUpdateDto updateCategory)
        {
            var existing = await _categoryRepository.GetCategoryByIdAsync(id.ToString());
            if (existing == null)
            {
                return null;
            }
            if (updateCategory.Name != null) existing.Name = updateCategory.Name;
            var newUpdateCategory = await _categoryRepository.UpdateCategoryAsync(existing);
            return newUpdateCategory != null ? new CategoryDto { Id = newUpdateCategory.Id, Name = newUpdateCategory.Name } : null;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id.ToString());
        }

    }
}
