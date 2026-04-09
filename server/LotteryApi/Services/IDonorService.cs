using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IDonorService
    {
        Task<DonorDto> CreateDonorsAsync(DonorCreateDto donor);
        Task<bool> DeleteDonorsAsync(int id);
        Task<IEnumerable<DonorDto>> GetDonorsAsync();
        Task<DonorDto?> GetDonorsByIdAsync(int id);
        Task<IEnumerable<DonorDto>> GetFilteredDonorsAsync(string? name, string? email, string? giftName);
        Task<DonorDto?> UpdateDonorsAsync(int id, DonorUpdateDto updateDonor);
    }
}