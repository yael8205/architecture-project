using LotteryApi.Data;
using LotteryApi.Dtos;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly LotteryDbContext _lotteryContext;
        public ShoppingCartRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }


        public async Task<ShoppingCartModel?> GetShoppingCartByIdAsync(int id)
        {
            return await _lotteryContext.ShoppingCarts
               .Include(p => p.Participant)
               .Include(pa => pa.PackagesInShoppingCart)
                     .ThenInclude(p => p.Package)
               .Include(pa => pa.PackagesInShoppingCart)
                .ThenInclude(g => g.GiftsInPackage)
                  .ThenInclude(g => g.Gift)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<ShoppingCartModel?> GetShoppingCartByUserIdAsync(int ParticipantId)
        {
            return await _lotteryContext.ShoppingCarts
               .Include(p => p.Participant)
               .Include(pa => pa.PackagesInShoppingCart)
                     .ThenInclude(p => p.Package)
               .Include(pa => pa.PackagesInShoppingCart)
                .ThenInclude(g => g.GiftsInPackage)
                  .ThenInclude(g => g.Gift)
                .FirstOrDefaultAsync(c => c.ParticipantId == ParticipantId);
        }
        public async Task<ShoppingCartModel> CreateShoppingCartAsync(ShoppingCartModel shoppingCart)
        {
            _lotteryContext.ShoppingCarts.Add(shoppingCart);
            await _lotteryContext.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCartModel?> UpdateShoppingCartAsync(ShoppingCartModel shoppingCart)
        {
            var existing = await _lotteryContext.ShoppingCarts.FindAsync(shoppingCart.Id);
            if (existing == null)
                return null;
            _lotteryContext.Entry(existing).CurrentValues.SetValues(shoppingCart);
            await _lotteryContext.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> DeleteShoppingCartAsync(int id)
        {
            var existing = await _lotteryContext.ShoppingCarts.FindAsync(id);
            if (existing == null)
                return false;
            _lotteryContext.ShoppingCarts.Remove(existing);
            await _lotteryContext.SaveChangesAsync();
            return true;

        }


        public async Task<bool> EmptyCartAsync(int cartId)
        {
            var cart = await _lotteryContext.ShoppingCarts
                .Include(c => c.PackagesInShoppingCart)
                  .ThenInclude(p => p.GiftsInPackage)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                return false;

            _lotteryContext.PackagesInCart.RemoveRange(cart.PackagesInShoppingCart);
            cart.SumPrice = 0;
            await _lotteryContext.SaveChangesAsync();
            return true;
        }
    }
}
