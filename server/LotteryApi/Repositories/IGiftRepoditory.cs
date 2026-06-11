using LotteryApi.Enums;
using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IGiftRepoditory
    {
        Task<GiftModel> CreateGiftAsync(GiftModel gift);
        Task<bool> DeleteGiftAsync(string id);
        Task<IEnumerable<GiftModel>> FilterGiftsAsync(int? categoryId, CardPriceEnum? priceType);
        Task<GiftModel?> GetGiftByIdAsync(string id);
        Task<IEnumerable<GiftModel>> GetGiftsAsync();
        Task<IEnumerable<GiftModel>> SearchGiftsAsync(string? giftName, string? donorName, int? minPurchasers);
        Task<IEnumerable<GiftModel>> SortedGiftsExpensiveAsync(string sortBy);
        Task<GiftModel?> UpdateGiftAsync(GiftModel gift);
    }
}