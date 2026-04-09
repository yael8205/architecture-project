using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateShoppingCartAsync(ShoppingCartDto shoppingcart);
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
    }
}