using LotteryApi.Models;

namespace LotteryApi.Repositories
{
    public interface IOrganizationRepository
    {
        Task AddAsync(Organization org);
        void Delete(Organization org);
        Task<IEnumerable<Organization>> GetAllAsync();
        Task<Organization?> GetByIdAsync(string id);
        Task<Organization?> GetBySlugAsync(string slug);
        Task<bool> SaveChangesAsync();
        void Update(Organization org);
    }
}