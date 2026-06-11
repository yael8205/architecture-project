using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class GiftInOrderRepositorycs : IGiftInOrderRepositorycs
    {
        private readonly IMongoCollection<GiftInOrderModel> _giftsInOrder;

        public GiftInOrderRepositorycs(IMongoCollection<GiftInOrderModel> giftsInOrder)
        {
            _giftsInOrder = giftsInOrder;
        }

        public async Task<GiftInOrderModel?> GetGiftInOrderByIdAsync(string id)
        {
            return await _giftsInOrder.Find(g => g.Id == id).FirstOrDefaultAsync();
        }
    }
}
