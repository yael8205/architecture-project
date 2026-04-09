using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDto> CreateShoppingCartAsync(ShoppingCartCreateDto shoppingcart);
        Task<bool> DeleteShoppingCartAsync(int id);
        Task<bool> EmptyCartAsync(int id);
        Task<ShoppingCartDto?> GetShoppingCartByIdAsync(int id);
        Task<ShoppingCartDto?> GetShoppingCartByUserIdAsync(int id);
    }
}