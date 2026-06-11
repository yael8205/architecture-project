using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IPackageInOrderRepository
    {
        Task<PackageInOrderModel?> GetPackageInOrderByIdAsync(string id);
    }
}