using LotteryApi.Data;
using LotteryApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LotteryApi.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly LotteryDbContext _lotteryContext;
        public OrganizationRepository(LotteryDbContext lotteryContext) => _lotteryContext = lotteryContext;

        public async Task<IEnumerable<Organization>> GetAllAsync() => await _lotteryContext.Organizations.ToListAsync();
        public async Task<Organization?> GetByIdAsync(int id) => await _lotteryContext.Organizations.FindAsync(id);
        public async Task<Organization?> GetBySlugAsync(string slug) => await _lotteryContext.Organizations.FirstOrDefaultAsync(o => o.Slug == slug);
        public async Task AddAsync(Organization org) => await _lotteryContext.Organizations.AddAsync(org);
        public void Update(Organization org) => _lotteryContext.Organizations.Update(org);
        public void Delete(Organization org) => _lotteryContext.Organizations.Remove(org);
        public async Task<bool> SaveChangesAsync() => (await _lotteryContext.SaveChangesAsync()) > 0;
    }
}
