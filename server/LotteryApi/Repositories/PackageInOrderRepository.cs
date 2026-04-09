using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class PackageInOrderRepository : IPackageInOrderRepository
    {
        private readonly LotteryDbContext _lotteryContext;
        public PackageInOrderRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }

        public async Task<PackageInOrderModel?> GetPackageInOrderByIdAsync(int id)
        {
            return await _lotteryContext.PackagesInOrder
                .Include(x => x.Package)
                .Include(p => p.GiftsInPackage)
                 .ThenInclude(g => g.Gift)
               .FirstOrDefaultAsync(p => p.Id == id);
        }
        //public async Task<bool> CreatePackagesAndGiftsInCartAsync(List<PackageInOrderModel>packages)
        //{

        //    _lotteryContext.PackagesInOrder.AddRange(packages);

        //    await _lotteryContext.SaveChangesAsync();
        //    return true;
        //}
    }
}
