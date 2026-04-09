using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IGiftInCartRepository
    {
        Task<GiftInCartModel> CreateGiftInCartAsync(GiftInCartModel giftInCart);
        Task<bool> DeleteGiftInCartAsync(int id);
        Task<GiftInCartModel?> GetGiftInCartByIdAndByPackageAsync(int giftid, int packageInCartId);
        Task<GiftInCartModel?> GetGiftInCartByIdAsync(int id);
        Task<GiftInCartModel?> UpdateGiftAsync(GiftInCartModel giftInCart);
    }
}