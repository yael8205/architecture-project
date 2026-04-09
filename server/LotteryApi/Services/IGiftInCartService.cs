using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IGiftInCartService
    {
        Task<GiftInCartDto> CreateOrUpdateGiftInCartAsync(GiftInCartCreateDto giftInCart);
        Task<bool> DeleteGiftInCartAsync(int id);
        Task<GiftInCartDto?> GetGiftInCartByIdAndByPackageAsync(int giftid, int packageInCartId);
        Task<GiftInCartDto?> GetGiftInCartByIdAsync(int id);
    }
}