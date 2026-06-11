using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class GiftInCartRepository : IGiftInCartRepository
    {
        private readonly IMongoCollection<GiftInCartModel> _giftsInCart;

        public GiftInCartRepository(IMongoCollection<GiftInCartModel> giftsInCart)
        {
            _giftsInCart = giftsInCart;
        }

        public async Task<GiftInCartModel?> GetGiftInCartByIdAsync(string id)
        {
            return await _giftsInCart.Find(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task<GiftInCartModel?> GetGiftInCartByIdAndByPackageAsync(string giftid, string packageInCartId)
        {
            return await _giftsInCart.Find(g => g.GiftId == giftid && g.PackageInCartId == packageInCartId).FirstOrDefaultAsync();
        }

        public async Task<GiftInCartModel> CreateGiftInCartAsync(GiftInCartModel giftInCart)
        {
            await _giftsInCart.InsertOneAsync(giftInCart);
            return giftInCart;
        }

        public async Task<GiftInCartModel?> UpdateGiftAsync(GiftInCartModel giftInCart)
        {
            var result = await _giftsInCart.ReplaceOneAsync(g => g.Id == giftInCart.Id, giftInCart);
            return result.MatchedCount == 0 ? null : giftInCart;
        }

        public async Task<bool> DeleteGiftInCartAsync(string id)
        {
            var result = await _giftsInCart.DeleteOneAsync(g => g.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
