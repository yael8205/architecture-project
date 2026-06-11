using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<OrderModel> _orders;

        public OrderRepository(IMongoCollection<OrderModel> orders)
        {
            _orders = orders;
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersAsync()
        {
            return await _orders.Find(_ => true).ToListAsync();
        }

        public async Task<OrderModel?> GetOrderByIdAsync(string id)
        {
            return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }
    }
}
