using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCartModel> CreateShoppingCartAsync(ShoppingCartModel shoppingCart);
        Task<bool> DeleteShoppingCartAsync(string id);
        Task<bool> EmptyCartAsync(string cartId);
        Task<ShoppingCartModel?> GetShoppingCartByIdAsync(string id);
        Task<ShoppingCartModel?> GetShoppingCartByUserIdAsync(string participantId);
        Task<ShoppingCartModel?> UpdateShoppingCartAsync(ShoppingCartModel shoppingCart);
    }
}