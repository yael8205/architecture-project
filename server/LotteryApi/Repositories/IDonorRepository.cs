using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IDonorRepository
    {
        Task<DonorModel> CreateDonorsAsync(DonorModel donor);
        Task<bool> DeleteDonorsAsync(string id);
        Task<IEnumerable<DonorModel>> GetDonorsAsync();
        Task<DonorModel?> GetDonorsByIdAsync(string id);
        Task<IEnumerable<DonorModel>> GetFilteredDonorsAsync(string? name, string? email, string? giftName);
        Task<DonorModel?> UpdateDonorsAsync(DonorModel donor);
    }
}