using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IPakageInOrderService
    {
        Task<PackageInOrderDto?> GetPackageInOrderByIdAsync(int id);
    }
}