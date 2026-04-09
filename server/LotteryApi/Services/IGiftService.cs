using LotteryApi.Dtos;
using LotteryApi.Enums;

namespace LotteryApi.Services
{
    public interface IGiftService
    {
        Task<GiftDto> CreateGiftAsync(GiftCreateDto gift);
        Task<bool> DeleteGiftAsync(int id);
        Task<IEnumerable<GiftDto>> FilteredGiftsAsync(int? categoryId, CardPriceEnum? priceType);
        Task<GiftDto?> GetGiftByIdAsync(int id);
        Task<IEnumerable<GiftDto>> GetGiftsAsync();
        Task<IEnumerable<GiftDto>> SearchGiftsAsync(string? giftName, string? donorName, int? minPurchasers);
        Task<IEnumerable<GiftDto>> SortedGiftsExpensiveAsync(string sortBy);
        Task<GiftDto?> UpdateGiftAsync(int id, GiftUpdateDto updateGift);
    }
}