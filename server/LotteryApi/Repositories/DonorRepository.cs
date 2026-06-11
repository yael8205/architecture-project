using LotteryApi.Models;
using MongoDB.Driver;
using System.Linq;

namespace LotteryApi.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly IMongoCollection<DonorModel> _donors;

        public DonorRepository(IMongoCollection<DonorModel> donors)
        {
            _donors = donors;
        }

        public async Task<IEnumerable<DonorModel>> GetDonorsAsync()
        {
            return await _donors.Find(_ => true).ToListAsync();
        }

        public async Task<DonorModel?> GetDonorsByIdAsync(string id)
        {
            return await _donors.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<DonorModel> CreateDonorsAsync(DonorModel donor)
        {
            await _donors.InsertOneAsync(donor);
            return donor;
        }

        public async Task<DonorModel?> UpdateDonorsAsync(DonorModel donor)
        {
            var result = await _donors.ReplaceOneAsync(d => d.Id == donor.Id, donor);
            return result.MatchedCount == 0 ? null : donor;
        }

        public async Task<bool> DeleteDonorsAsync(string id)
        {
            var result = await _donors.DeleteOneAsync(d => d.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<DonorModel>> GetFilteredDonorsAsync(string? name, string? email, string? giftName)
        {
            var filter = Builders<DonorModel>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(name))
                filter &= Builders<DonorModel>.Filter.Regex(d => d.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

            if (!string.IsNullOrWhiteSpace(email))
                filter &= Builders<DonorModel>.Filter.Regex(d => d.Email, new MongoDB.Bson.BsonRegularExpression(email, "i"));

            if (!string.IsNullOrWhiteSpace(giftName))
                filter &= Builders<DonorModel>.Filter.ElemMatch(d => d.Gifts, Builders<LotteryApi.Models.GiftModel>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(giftName, "i")));

            return await _donors.Find(filter).ToListAsync();
        }
    }
}
