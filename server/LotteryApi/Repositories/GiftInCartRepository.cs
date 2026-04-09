using LotteryApi.Data;
using LotteryApi.Dtos;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class GiftInCartRepository : IGiftInCartRepository
    {
        private readonly LotteryDbContext _lotteryContext ;
        public GiftInCartRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }

        public async Task<GiftInCartModel?> GetGiftInCartByIdAsync(int id)
        {
            return await _lotteryContext.GiftsInCart
              .Include(g => g.Gift)
              .Include(g => g.PackageInCart)
              .FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task<GiftInCartModel?> GetGiftInCartByIdAndByPackageAsync(int giftid, int packageInCartId)
        {
            return await _lotteryContext.GiftsInCart
              .Include(g => g.Gift)
              .Include(g => g.PackageInCart)
              .FirstOrDefaultAsync(g => g.GiftId == giftid && g.PackageInCartId == packageInCartId);
        }
        public async Task<GiftInCartModel> CreateGiftInCartAsync(GiftInCartModel giftInCart)
        {
            _lotteryContext.GiftsInCart.Add(giftInCart);
            await _lotteryContext.SaveChangesAsync();
            return giftInCart;
        }


        public async Task<GiftInCartModel?> UpdateGiftAsync(GiftInCartModel giftInCart)
        {
            var existing = await _lotteryContext.GiftsInCart.FindAsync(giftInCart.Id);
            if (existing == null)
                return null;
            existing.Qty = giftInCart.Qty;
            await _lotteryContext.SaveChangesAsync();
            return existing;

        }

        public async Task<bool> DeleteGiftInCartAsync(int id)
        {
            var existing = await _lotteryContext.GiftsInCart.FindAsync(id);
            if (existing == null)
                return false;
            _lotteryContext.GiftsInCart.Remove(existing);
            await _lotteryContext.SaveChangesAsync();
            return true;
        }
    }
}
