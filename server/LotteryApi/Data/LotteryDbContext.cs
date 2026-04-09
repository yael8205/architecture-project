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

        public DbSet<DonorModel> Donors { get; set; }
        public DbSet<GiftModel> Gifts { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PackageModel> Packages { get; set; }
        public DbSet<GiftInOrderModel> GiftsInOrder { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<GiftInCartModel> GiftsInCart { get; set; }
        public DbSet<ShoppingCartModel> ShoppingCarts { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<PackageInCartModel> PackagesInCart { get; set; }
        public DbSet<PackageInOrderModel> PackagesInOrder { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // בדיקה חשובה: אם ה-Provider קיים, נחיל את הפילטרים. 
            // אם הוא null (בזמן Migration), נדלג עליהם כדי שלא תקרוס המערכת.
            if (_tenantProvider != null)
            {
                // הערה: אנחנו משתמשים ישירות במתודה בתוך הלמבדה
                modelBuilder.Entity<CategoryModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<GiftModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<DonorModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<UserModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<OrderModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<PackageModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<ShoppingCartModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<GiftInOrderModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
                modelBuilder.Entity<PackageInOrderModel>().HasQueryFilter(e => e.OrganizationId == _tenantProvider.GetTenantId());
            }

            // --- הקשרים שלך (נשארים ללא שינוי) ---
            modelBuilder.Entity<PackageInCartModel>()
                .HasOne(p => p.ShoppingCart)
                .WithMany(s => s.PackagesInShoppingCart)
                .HasForeignKey(p => p.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GiftInCartModel>()
                .HasOne(g => g.PackageInCart)
                .WithMany(p => p.GiftsInPackage)
                .HasForeignKey(g => g.PackageInCartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GiftInCartModel>()
                .HasOne(g => g.ShoppingCart)
                .WithMany()
                .HasForeignKey(g => g.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GiftInOrderModel>()
                .HasOne(g => g.PackageInOrder)
                .WithMany(p => p.GiftsInPackage)
                .HasForeignKey(g => g.PackageInOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // גם כאן, נוודא שה-Provider קיים לפני הזרקת ה-ID
            if (_tenantProvider != null)
            {
                var entries = ChangeTracker.Entries<ITenantEntity>()
                    .Where(e => e.State == EntityState.Added);

                int currentId = _tenantProvider.GetTenantId();

                foreach (var entry in entries)
                {
                    entry.Entity.OrganizationId = currentId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}