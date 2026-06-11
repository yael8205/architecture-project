using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly IMongoCollection<PackageModel> _packages;

        public PackageRepository(IMongoCollection<PackageModel> packages)
        {
            _packages = packages;
        }

        public async Task<IEnumerable<PackageModel>> GetPackageAsync()
        {
            return await _packages.Find(_ => true).ToListAsync();
        }

        public async Task<PackageModel?> GetPackageByIdAsync(string id)
        {
            return await _packages.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PackageModel> CreatePackageAsync(PackageModel package)
        {
            await _packages.InsertOneAsync(package);
            return package;
        }

        public async Task<PackageModel?> UpdatePackageAsync(PackageModel package)
        {
            var result = await _packages.ReplaceOneAsync(p => p.Id == package.Id, package);
            return result.MatchedCount == 0 ? null : package;
        }

        public async Task<bool> DeletePackageAsync(string id)
        {
            var result = await _packages.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }
    }
}

