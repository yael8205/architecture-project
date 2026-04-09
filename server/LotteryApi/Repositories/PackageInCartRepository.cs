using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class PackageInCartRepository : IPackageInCartRepository
    {
        private readonly LotteryDbContext _lotteryContext ;

        public PackageInCartRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }

        public async Task<PackageInCartModel?> GetPackageInCartByIdAsync(int id)
        {
            return await _lotteryContext.PackagesInCart
                .Include(p => p.Package)
                .Include(p => p.GiftsInPackage)
                 .ThenInclude(g => g.Gift)
               .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<PackageInCartModel> CreatePackageInCartAsync(PackageInCartModel packageInCart)
        {
            _lotteryContext.PackagesInCart.Add(packageInCart);
            await _lotteryContext.SaveChangesAsync();
            return packageInCart;
        }
        public async Task<bool> DeletePackageInCartAsync(int id)
        {
            var existing = await _lotteryContext.PackagesInCart.FindAsync(id);
            if (existing == null)
                return false;
            _lotteryContext.PackagesInCart.Remove(existing);
            await _lotteryContext.SaveChangesAsync();
            return true;
        }
    }
}
