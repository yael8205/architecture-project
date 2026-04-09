using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IPackageInOrderRepository
    {
        Task<PackageInOrderModel?> GetPackageInOrderByIdAsync(int id);
    }
}