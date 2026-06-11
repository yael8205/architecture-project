using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IPackageInCartRepository
    {
        Task<PackageInCartModel> CreatePackageInCartAsync(PackageInCartModel packageInCart);
        Task<bool> DeletePackageInCartAsync(string id);
        Task<PackageInCartModel?> GetPackageInCartByIdAsync(string id);
    }
}