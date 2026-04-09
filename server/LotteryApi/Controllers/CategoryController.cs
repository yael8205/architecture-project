using LotteryApi.Dtos;
using LotteryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }
            return Ok(category);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategoryAsync([FromBody] CategoryCreateDto category)
        {
            var newCategory=await _categoryService.CreateCategoryAsync(category);
            return Ok(newCategory);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategoryAsync(int id, [FromBody] CategoryUpdateDto category)
        {
            var updateCategory = await _categoryService.UpdateCategoryAsync(id, category);
            if (updateCategory == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }
            return Ok(updateCategory);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            var isDeleted = await _categoryService.DeleteCategoryAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }
            return NoContent();
        }
    
}
}
