using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IPackageService
    {
        Task<PackageDto> CreatePackageAsync(PackageCreateDto package);
        Task<bool> DeletePackageAsync(int id);
        Task<IEnumerable<PackageDto>> GetPackageAsync();
        Task<PackageDto?> GetPackageByIdAsync(int id);
        Task<PackageDto?> UpdatePackageAsync(int id, PackageUpdateDto updatePackage);
    }
}