using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class PackageInCartRepository : IPackageInCartRepository
    {
        private readonly IMongoCollection<PackageInCartModel> _packagesInCart;

        public PackageInCartRepository(IMongoCollection<PackageInCartModel> packagesInCart)
        {
            _packagesInCart = packagesInCart;
        }

        public async Task<PackageInCartModel?> GetPackageInCartByIdAsync(string id)
        {
            return await _packagesInCart.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PackageInCartModel> CreatePackageInCartAsync(PackageInCartModel packageInCart)
        {
            await _packagesInCart.InsertOneAsync(packageInCart);
            return packageInCart;
        }

        public async Task<bool> DeletePackageInCartAsync(string id)
        {
            var result = await _packagesInCart.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
