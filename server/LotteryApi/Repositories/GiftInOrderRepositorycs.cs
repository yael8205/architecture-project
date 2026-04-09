using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class GiftInOrderRepositorycs : IGiftInOrderRepositorycs
    {
        private readonly LotteryDbContext _lotteryContext;
        public GiftInOrderRepositorycs(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }
        public async Task<GiftInOrderModel?> GetGiftInOrderByIdAsync(int id)
        {
            return await _lotteryContext.GiftsInOrder
            .Include(g => g.Gift)
            .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
