using LotteryApi.Data;
using LotteryApi.Dtos;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly LotteryDbContext _lotteryContext;
        public DonorRepository(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }
        public async Task<IEnumerable<DonorModel>> GetDonorsAsync()
        {
            return await _lotteryContext.Donors
                .Include(d => d.Gifts)
                .ToListAsync();
        }
        public async Task<DonorModel?> GetDonorsByIdAsync(int id)
        {
            return await _lotteryContext.Donors
                 .Include(d => d.Gifts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<DonorModel> CreateDonorsAsync(DonorModel donor)
        {
            _lotteryContext.Donors.Add(donor);
            await _lotteryContext.SaveChangesAsync();
            return donor;
        }
        public async Task<DonorModel?> UpdateDonorsAsync(DonorModel donor)
        {
            var existing = await _lotteryContext.Donors.FindAsync(donor.Id);
            if (existing == null)
                return null;
            _lotteryContext.Entry(existing).CurrentValues.SetValues(donor);
            await _lotteryContext.SaveChangesAsync();
            return existing;

        }
        public async Task<bool> DeleteDonorsAsync(int id)
        {
            var existing = await _lotteryContext.Donors.FindAsync(id);
            if (existing == null)
                return false;
            _lotteryContext.Donors.Remove(existing);
            await _lotteryContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<DonorModel>> GetFilteredDonorsAsync(string? name, string? email, string? giftName)
        {

            var query = _lotteryContext.Donors.Include(d => d.Gifts).AsQueryable();


            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(d => d.Email.Contains(email));

            if (!string.IsNullOrWhiteSpace(giftName))

                query = query.Where(d => d.Gifts.Any(g => g.Name.Contains(giftName)));


            return await query.ToListAsync();
        }
    }

}
