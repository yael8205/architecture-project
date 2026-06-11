using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IPackageRepository
    {
        Task<PackageModel> CreatePackageAsync(PackageModel package);
        Task<bool> DeletePackageAsync(string id);
        Task<IEnumerable<PackageModel>> GetPackageAsync();
        Task<PackageModel?> GetPackageByIdAsync(string id);
        Task<PackageModel?> UpdatePackageAsync(PackageModel package);
    }
}