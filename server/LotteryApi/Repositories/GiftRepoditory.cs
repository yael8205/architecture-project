using LotteryApi.Data;
using LotteryApi.Enums;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Repositories
{
    public class GiftRepoditory : IGiftRepoditory
    {
        private readonly LotteryDbContext _lotteryContext;
        public GiftRepoditory(LotteryDbContext lotteryDbContext)
        {
            _lotteryContext = lotteryDbContext;
        }
        public async Task<IEnumerable<GiftModel>> GetGiftsAsync()
        {
            return await _lotteryContext.Gifts
                .Include(c => c.Category)
                .Include(d => d.Donor)
                .Include(g => g.GifPurchased)
                .ThenInclude(p=>p.PackageInOrder)
                  .ThenInclude(o => o.Order)
                    .ThenInclude(u => u.Participant)
                .ToListAsync();
        }
        public async Task<GiftModel?> GetGiftByIdAsync(int id)
        {
            return await _lotteryContext.Gifts
                .Include(c => c.Category)
                .Include(d => d.Donor)
                 .Include(g => g.GifPurchased)
                 .ThenInclude(p => p.PackageInOrder)
                  .ThenInclude(o => o.Order)
                    .ThenInclude(u => u.Participant)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<GiftModel> CreateGiftAsync(GiftModel gift)
        {
            _lotteryContext.Gifts.Add(gift);
            await _lotteryContext.SaveChangesAsync();
            return gift;
        }
        public async Task<GiftModel?> UpdateGiftAsync(GiftModel gift)
        {
            var existing = await _lotteryContext.Gifts.FindAsync(gift.Id);
            if (existing == null)
                return null;
            _lotteryContext.Entry(existing).CurrentValues.SetValues(gift);
            await _lotteryContext.SaveChangesAsync();
            return existing;

        }
        public async Task<bool> DeleteGiftAsync(int id)
        {
            var existing = await _lotteryContext.Gifts.FindAsync(id);
            if (existing == null)
                return false;
            _lotteryContext.Gifts.Remove(existing);
            await _lotteryContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<GiftModel>> SearchGiftsAsync(string? giftName, string? donorName, int? minPurchasers)
        {
            var query = _lotteryContext.Gifts
                .Include(c => c.Category)
                .Include(d => d.Donor)
                .Include(g => g.GifPurchased)
                .ThenInclude(gp => gp.PackageInOrder)
                    .ThenInclude(go => go.Order)
                .AsQueryable();

            if (!string.IsNullOrEmpty(giftName))
            {
                query = query.Where(g => g.Name.Contains(giftName));
            }

            if (!string.IsNullOrEmpty(donorName))
            {
                query = query.Where(g => g.Donor != null && g.Donor.Name.Contains(donorName));
            }

            if (minPurchasers.HasValue)
            {
                query = query.Where(g => g.GifPurchased.Count >= minPurchasers.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<GiftModel>> FilterGiftsAsync(int? categoryId, CardPriceEnum? priceType)
        {
            var query = _lotteryContext.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.GifPurchased)
                .ThenInclude(gp => gp.PackageInOrder)
                    .ThenInclude(gp => gp.Order)
                    .ThenInclude(o => o.Participant)
                .AsQueryable();

            if (categoryId.HasValue && categoryId > 0)
                query = query.Where(g => g.CategoryId == categoryId.Value);

            if (priceType.HasValue)
                query = query.Where(g => g.CardPrice == priceType.Value);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<GiftModel>> SortedGiftsExpensiveAsync(string sortBy)
        {
            var query = _lotteryContext.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.GifPurchased)
                 .ThenInclude(gp => gp.PackageInOrder)
                    .ThenInclude(gp => gp.Order)
                    .ThenInclude(o => o.Participant)
                .AsQueryable();

            if (sortBy == "price")
            {
                query = query.OrderByDescending(g => g.CardPrice);
            }
            else if (sortBy == "popular")
            {

                query = query.OrderByDescending(g => g.GifPurchased.Count);
            }

            return await query.ToListAsync();
        }

    }
}
