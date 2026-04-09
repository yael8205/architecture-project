using LotteryApi.Dtos;
using LotteryApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService ;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartByIdAsync(int id)
        {
            var shoppingCart = await _shoppingCartService.GetShoppingCartByIdAsync(id);
            if (shoppingCart == null)
            {
                return NotFound(new { message = $"ShoppingCart with ID {id} not found." });
            }
            return Ok(shoppingCart);
        }
        [HttpGet("user/{id}")]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartByUserIdAsync(int id)
        {
            var shoppingCart = await _shoppingCartService.GetShoppingCartByUserIdAsync(id);
            if (shoppingCart == null)
            {
                return NotFound(new { message = $"ShoppingCart with UserID {id} not found." });
            }
            return Ok(shoppingCart);
        }
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> CreatePackageAsync([FromBody] ShoppingCartCreateDto shoppingCart)
        {
            var newShoppingCart = await _shoppingCartService.CreateShoppingCartAsync(shoppingCart);
            return Ok(newShoppingCart);
        }
   
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePackageAsync(int id)
        {
            var isDeleted = await _shoppingCartService.DeleteShoppingCartAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"ShoppingCart with ID {id} not found." });
            }
            return NoContent();
        }
        [HttpDelete("clear/{id}")]

        public async Task<ActionResult> EmptyCartAsync(int id)
        {
            var isEmpty = await _shoppingCartService.EmptyCartAsync(id);
            if (!isEmpty)
            {
                return NotFound(new { message = $"ShoppingCart with ID {id} not found." });
            }
            return NoContent();
        }

    }
}
