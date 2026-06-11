using LotteryApi.Enums;
using LotteryApi.Models;
using MongoDB.Driver;
using System.Linq;

namespace LotteryApi.Repositories
{
    public class GiftRepoditory : IGiftRepoditory
    {
        private readonly IMongoCollection<GiftModel> _gifts;

        public GiftRepoditory(IMongoCollection<GiftModel> gifts)
        {
            _gifts = gifts;
        }

        public async Task<IEnumerable<GiftModel>> GetGiftsAsync()
        {
            return await _gifts.Find(_ => true).ToListAsync();
        }

        public async Task<GiftModel?> GetGiftByIdAsync(string id)
        {
            return await _gifts.Find(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task<GiftModel> CreateGiftAsync(GiftModel gift)
        {
            await _gifts.InsertOneAsync(gift);
            return gift;
        }

        public async Task<GiftModel?> UpdateGiftAsync(GiftModel gift)
        {
            var result = await _gifts.ReplaceOneAsync(g => g.Id == gift.Id, gift);
            return result.MatchedCount == 0 ? null : gift;
        }

        public async Task<bool> DeleteGiftAsync(string id)
        {
            var result = await _gifts.DeleteOneAsync(g => g.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<GiftModel>> SearchGiftsAsync(string? giftName, string? donorName, int? minPurchasers)
        {
            var filter = Builders<GiftModel>.Filter.Empty;

            if (!string.IsNullOrEmpty(giftName))
                filter &= Builders<GiftModel>.Filter.Regex(g => g.Name, new MongoDB.Bson.BsonRegularExpression(giftName, "i"));

            if (!string.IsNullOrEmpty(donorName))
                filter &= Builders<GiftModel>.Filter.Regex("Donor.Name", new MongoDB.Bson.BsonRegularExpression(donorName, "i"));

            if (minPurchasers.HasValue)
                filter &= Builders<GiftModel>.Filter.SizeGte(g => g.GifPurchased, minPurchasers.Value);

            return await _gifts.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<GiftModel>> FilterGiftsAsync(int? categoryId, CardPriceEnum? priceType)
        {
            var filter = Builders<GiftModel>.Filter.Empty;

            if (categoryId.HasValue && categoryId > 0)
                filter &= Builders<GiftModel>.Filter.Eq(g => g.CategoryId, categoryId.Value.ToString());

            if (priceType.HasValue)
                filter &= Builders<GiftModel>.Filter.Eq(g => g.CardPrice, priceType.Value);

            return await _gifts.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<GiftModel>> SortedGiftsExpensiveAsync(string sortBy)
        {
            var sortDefinition = sortBy == "price"
                ? Builders<GiftModel>.Sort.Descending(g => g.CardPrice)
                : Builders<GiftModel>.Sort.Descending(g => g.GifPurchased.Count);

            return await _gifts.Find(_ => true).Sort(sortDefinition).ToListAsync();
        }
    }
}
