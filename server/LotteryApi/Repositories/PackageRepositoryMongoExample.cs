using LotteryApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class PackageRepositoryMongoExample
    {
        private readonly IMongoCollection<PackageModel> _packages;

        public PackageRepositoryMongoExample(IMongoCollection<PackageModel> packages)
        {
            _packages = packages;
        }

        public async Task<IEnumerable<PackageModel>> GetPackageAsync()
        {
            return await _packages.Find(Builders<PackageModel>.Filter.Empty)
                .ToListAsync();
        }

        public async Task<PackageModel?> GetPackageByIdAsync(string id)
        {
            var filter = Builders<PackageModel>.Filter.Eq(x => x.Id, id);
            return await _packages.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<PackageModel> CreatePackageAsync(PackageModel package)
        {
            await _packages.InsertOneAsync(package);
            return package;
        }

        public async Task<PackageModel?> UpdatePackageAsync(PackageModel package)
        {
            var filter = Builders<PackageModel>.Filter.Eq(x => x.Id, package.Id);
            var result = await _packages.ReplaceOneAsync(filter, package);
            return result.IsAcknowledged && result.ModifiedCount > 0 ? package : null;
        }

        public async Task<bool> DeletePackageAsync(string id)
        {
            var filter = Builders<PackageModel>.Filter.Eq(x => x.Id, id);
            var result = await _packages.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
