using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IMongoCollection<ShoppingCartModel> _shoppingCarts;
        private readonly IMongoCollection<PackageInCartModel> _packagesInCart;

        public ShoppingCartRepository(
            IMongoCollection<ShoppingCartModel> shoppingCarts,
            IMongoCollection<PackageInCartModel> packagesInCart)
        {
            _shoppingCarts = shoppingCarts;
            _packagesInCart = packagesInCart;
        }

        public async Task<ShoppingCartModel?> GetShoppingCartByIdAsync(string id)
        {
            return await _shoppingCarts.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ShoppingCartModel?> GetShoppingCartByUserIdAsync(string participantId)
        {
            return await _shoppingCarts.Find(c => c.ParticipantId == participantId).FirstOrDefaultAsync();
        }

        public async Task<ShoppingCartModel> CreateShoppingCartAsync(ShoppingCartModel shoppingCart)
        {
            await _shoppingCarts.InsertOneAsync(shoppingCart);
            return shoppingCart;
        }

        public async Task<ShoppingCartModel?> UpdateShoppingCartAsync(ShoppingCartModel shoppingCart)
        {
            var result = await _shoppingCarts.ReplaceOneAsync(c => c.Id == shoppingCart.Id, shoppingCart);
            return result.MatchedCount == 0 ? null : shoppingCart;
        }

        public async Task<bool> DeleteShoppingCartAsync(string id)
        {
            var result = await _shoppingCarts.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> EmptyCartAsync(string cartId)
        {
            var cart = await _shoppingCarts.Find(c => c.Id == cartId).FirstOrDefaultAsync();
            if (cart == null)
                return false;

            await _packagesInCart.DeleteManyAsync(p => p.CartId == cart.Id);
            cart.SumPrice = 0;
            await _shoppingCarts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return true;
        }
    }
}
