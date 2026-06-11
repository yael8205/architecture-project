using LotteryApi.Models;
using LotteryApi.Services;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Data
{
    public class LotteryDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public LotteryDbContext(DbContextOptions<LotteryDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }
    }
}