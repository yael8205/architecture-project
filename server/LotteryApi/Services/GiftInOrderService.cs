using LotteryApi.Dtos;
using LotteryApi.Repositories;
using System.Runtime.CompilerServices;

namespace LotteryApi.Services
{
    public class GiftInOrderService : IGiftInOrderService
    {
        private readonly IGiftInOrderRepositorycs _giftInOrderRepositorycs;
        public GiftInOrderService(IGiftInOrderRepositorycs giftInOrderRepositorycs)
        {
            _giftInOrderRepositorycs = giftInOrderRepositorycs;
        }

        public async Task<GiftInOrderDto?> GetGiftInOrderByIdAsync(int id)
        {
            var giftInOrder = await _giftInOrderRepositorycs.GetGiftInOrderByIdAsync(id);
            return giftInOrder != null ? new GiftInOrderDto
            {
                Id = giftInOrder.Id,
                GiftId = giftInOrder.GiftId,
                GiftName = giftInOrder.Gift?.Name,
                GiftPictureUrl = giftInOrder.Gift?.PictureUrl,
                GiftCardPrice = giftInOrder.Gift?.CardPrice.ToString(),
                IsWinner = giftInOrder.IsWinner
            } : null;

        }
    }
}
