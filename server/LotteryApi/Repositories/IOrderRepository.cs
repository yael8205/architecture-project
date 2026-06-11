using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderModel> CreateOrderAsync(OrderModel order);
        Task<OrderModel?> GetOrderByIdAsync(string id);
        Task<IEnumerable<OrderModel>> GetOrdersAsync();
    }
}