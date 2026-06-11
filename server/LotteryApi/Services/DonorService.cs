using LotteryApi.Dtos;
using LotteryApi.Models;
using LotteryApi.Repositories;
using System.Drawing;

namespace LotteryApi.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;
        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }
        public async Task<IEnumerable<DonorDto>> GetDonorsAsync()
        {
            var donors = await _donorRepository.GetDonorsAsync();
            return donors.Select(d => new DonorDto
            {
                Id = d.Id,
                Name = d.Name,
                Phone = d.Phone,
                Email = d.Email,
                GiftNames = d.Gifts.Select(g => g.Name).ToList()
            });
        }
        public async Task<DonorDto?> GetDonorsByIdAsync(int id)
        {
            var donor = await _donorRepository.GetDonorsByIdAsync(id.ToString());
            return donor != null ? new DonorDto { Id = donor.Id, Name = donor.Name, Email = donor.Email, Phone = donor.Phone, GiftNames = donor.Gifts.Select(g => g.Name).ToList() } : null;
        }

        public async Task<DonorDto> CreateDonorsAsync(DonorCreateDto donor)
        {
            var newDonor = new DonorModel
            {
                Name = donor.Name,
                Email = donor.Email,
                Phone = donor.Phone,
            };

            var createDonor = await _donorRepository.CreateDonorsAsync(newDonor);
            return new DonorDto { Id = createDonor.Id, Name = createDonor.Name, Email = createDonor.Email, Phone = createDonor.Phone, GiftNames = createDonor.Gifts.Select(g => g.Name).ToList() };
        }

        public async Task<DonorDto?> UpdateDonorsAsync(int id, DonorUpdateDto updateDonor)
        {
            var existing = await _donorRepository.GetDonorsByIdAsync(id.ToString());
            if (existing == null)
            {
                return null;
            }

            if (updateDonor.Name != null) existing.Name = updateDonor.Name;
            if (updateDonor.Email != null) existing.Email = updateDonor.Email;
            if (updateDonor.Phone != null) existing.Phone = updateDonor.Phone;
            var newUpdateDonor = await _donorRepository.UpdateDonorsAsync(existing);
            return newUpdateDonor != null ? new DonorDto { Id = newUpdateDonor.Id, Name = newUpdateDonor.Name, Email = newUpdateDonor.Email, Phone = newUpdateDonor.Phone, GiftNames = newUpdateDonor.Gifts.Select(g => g.Name).ToList() } : null;
        }
        public async Task<bool> DeleteDonorsAsync(int id)
        {
            return await _donorRepository.DeleteDonorsAsync(id.ToString());
        }
        public async Task<IEnumerable<DonorDto>> GetFilteredDonorsAsync(string? name, string? email, string? giftName)
        {
            var donors = await _donorRepository.GetFilteredDonorsAsync(name, email, giftName);

            return donors.Select(d => new DonorDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                Phone = d.Phone,
                GiftNames = d.Gifts.Select(g => g.Name).ToList()
            });
        }
    }
}
