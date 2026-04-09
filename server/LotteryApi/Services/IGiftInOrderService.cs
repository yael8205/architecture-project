using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IGiftInOrderService
    {
        Task<GiftInOrderDto?> GetGiftInOrderByIdAsync(int id);
    }
}