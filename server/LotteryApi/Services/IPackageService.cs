using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IPackageService
    {
        Task<PackageDto> CreatePackageAsync(PackageCreateDto package);
        Task<bool> DeletePackageAsync(string id);
        Task<IEnumerable<PackageDto>> GetPackageAsync();
        Task<PackageDto?> GetPackageByIdAsync(string id);
        Task<PackageDto?> UpdatePackageAsync(string id, PackageUpdateDto updatePackage);
    }
}