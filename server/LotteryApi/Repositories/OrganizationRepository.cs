using LotteryApi.Models;
using MongoDB.Driver;

namespace LotteryApi.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IMongoCollection<Organization> _organizations;

        public OrganizationRepository(IMongoCollection<Organization> organizations)
        {
            _organizations = organizations;
        }

        public async Task<IEnumerable<Organization>> GetAllAsync() => await _organizations.Find(_ => true).ToListAsync();

        public async Task<Organization?> GetByIdAsync(string id) => await _organizations.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task<Organization?> GetBySlugAsync(string slug) => await _organizations.Find(o => o.Slug == slug).FirstOrDefaultAsync();

        public async Task AddAsync(Organization org) => await _organizations.InsertOneAsync(org);

        public void Update(Organization org) => _organizations.ReplaceOneAsync(o => o.Id == org.Id, org).GetAwaiter().GetResult();

        public void Delete(Organization org) => _organizations.DeleteOneAsync(o => o.Id == org.Id).GetAwaiter().GetResult();

        public async Task<bool> SaveChangesAsync() => true;
    }
}
