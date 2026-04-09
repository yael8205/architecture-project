using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IPackageRepository
    {
        Task<PackageModel> CreatePackageAsync(PackageModel package);
        Task<bool> DeletePackageAsync(int id);
        Task<IEnumerable<PackageModel>> GetPackageAsync();
        Task<PackageModel?> GetPackageByIdAsync(int id);
        Task<PackageModel?> UpdatePackageAsync(PackageModel package);
    }
}