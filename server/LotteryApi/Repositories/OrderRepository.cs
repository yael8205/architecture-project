using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LotteryDbContext _lotteryContext;
        public OrderRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersAsync()
        {
            return await _lotteryContext.Orders
                .Include(p => p.PackagesInOrder)
                  .ThenInclude(p => p.Package)
                .Include(p => p.PackagesInOrder)
                 .ThenInclude(g => g.GiftsInPackage)
                   .ThenInclude(g => g.Gift)
                .ToListAsync();
        }
        public async Task<OrderModel?> GetOrderByIdAsync(int id)
        {
            return await _lotteryContext.Orders
               .Include(p => p.PackagesInOrder)
                  .ThenInclude(p => p.Package)
                .Include(p => p.PackagesInOrder)
                 .ThenInclude(g => g.GiftsInPackage)
                   .ThenInclude(g => g.Gift)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            _lotteryContext.Orders.Add(order);
            await _lotteryContext.SaveChangesAsync();
            return order;
        }
    }
}
