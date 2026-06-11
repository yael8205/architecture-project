using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IGiftInOrderRepositorycs
    {
        Task<GiftInOrderModel?> GetGiftInOrderByIdAsync(string id);
    }
}