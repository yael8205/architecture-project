using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IGiftInCartRepository
    {
        Task<GiftInCartModel> CreateGiftInCartAsync(GiftInCartModel giftInCart);
        Task<bool> DeleteGiftInCartAsync(string id);
        Task<GiftInCartModel?> GetGiftInCartByIdAndByPackageAsync(string giftid, string packageInCartId);
        Task<GiftInCartModel?> GetGiftInCartByIdAsync(string id);
        Task<GiftInCartModel?> UpdateGiftAsync(GiftInCartModel giftInCart);
    }
}