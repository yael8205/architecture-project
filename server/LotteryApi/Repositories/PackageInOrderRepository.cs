using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class PackageInOrderRepository : IPackageInOrderRepository
    {
        private readonly IMongoCollection<PackageInOrderModel> _packagesInOrder;

        public PackageInOrderRepository(IMongoCollection<PackageInOrderModel> packagesInOrder)
        {
            _packagesInOrder = packagesInOrder;
        }

        public async Task<PackageInOrderModel?> GetPackageInOrderByIdAsync(string id)
        {
            return await _packagesInOrder.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}
