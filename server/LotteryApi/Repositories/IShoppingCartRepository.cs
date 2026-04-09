using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCartModel> CreateShoppingCartAsync(ShoppingCartModel shoppingCart);
        Task<bool> DeleteShoppingCartAsync(int id);
        Task<bool> EmptyCartAsync(int cartId);
        Task<ShoppingCartModel?> GetShoppingCartByIdAsync(int id);
        Task<ShoppingCartModel?> GetShoppingCartByUserIdAsync(int ParticipantId);
        Task<ShoppingCartModel?> UpdateShoppingCartAsync(ShoppingCartModel shoppingCart);
    }
}