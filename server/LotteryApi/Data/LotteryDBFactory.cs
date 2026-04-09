using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LotteryApi.Data
{
    // שימוש בממשק הרשמי של EF Core ליצירת Context בזמן פיתוח
    public class LotteryDbContextFactory : IDesignTimeDbContextFactory<LotteryDbContext>
    {
        private const string ConnectionString = "Server=DESKTOP-PKVNNGR;DataBase=LotteryDB;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=true";

        public LotteryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LotteryDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            // אנחנו שולחים null עבור ה-ITenantProvider כי בזמן יצירת Migration 
            // אין HTTP Request ואין טוקן של משתמש. 
            // ה-! אומר למהדר שאנחנו מודעים לכך שזה null.
            return new LotteryDbContext(optionsBuilder.Options, null!);
        }
    }
}