using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class PackageRepository : IPackageRepository
    {

        private readonly LotteryDbContext _lotteryContext;
        public PackageRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }
        public async Task<IEnumerable<PackageModel>> GetPackageAsync()
        {
            return await _lotteryContext.Packages.ToListAsync();
        }
        public async Task<PackageModel?> GetPackageByIdAsync(int id)
        {
            return await _lotteryContext.Packages
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<PackageModel> CreatePackageAsync(PackageModel package)
        {
            _lotteryContext.Packages.Add(package);
            await _lotteryContext.SaveChangesAsync();
            return package;
        }
        public async Task<PackageModel?> UpdatePackageAsync(PackageModel package)
        {
            var existing = await _lotteryContext.Packages.FindAsync(package.Id);
            if (existing == null)
                return null;
            _lotteryContext.Entry(existing).CurrentValues.SetValues(package);
            await _lotteryContext.SaveChangesAsync();
            return existing;

        }
        public async Task<bool> DeletePackageAsync(int id)
        {
            var existing = await _lotteryContext.Packages.FindAsync(id);
            if (existing == null)
                return false;
            _lotteryContext.Packages.Remove(existing);
            await _lotteryContext.SaveChangesAsync();
            return true;
        }

    }
}

