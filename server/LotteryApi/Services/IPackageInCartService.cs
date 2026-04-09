using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IPackageInCartService
    {
        Task<PackageInCartDto> CreatePackageInCartAsync(PackageInCartCreateDto packageInCart);
        Task<bool> DeletePackageInCartAsync(int id);
        Task<PackageInCartDto?> GetPackageInCartByIdAsync(int id);
    }
}