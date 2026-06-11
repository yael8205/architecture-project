using LotteryApi.Dtos;
using LotteryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class OrsersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrsersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersAsync()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = $"Order with ID {id} not found." });
            }
            return Ok(order);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateShoppingCartAsync([FromBody] ShoppingCartDto shoppingCart)
        {
            var newOrder = await _orderService.CreateShoppingCartAsync(shoppingCart);
            return Ok(newOrder);
        }
    }
}
