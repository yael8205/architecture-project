using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IPackageInCartRepository
    {
        Task<PackageInCartModel> CreatePackageInCartAsync(PackageInCartModel packageInCart);
        Task<bool> DeletePackageInCartAsync(int id);
        Task<PackageInCartModel?> GetPackageInCartByIdAsync(int id);
    }
}