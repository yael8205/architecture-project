namespace LotteryApi.Services
{
    public interface ITenantProvider
    {
        int GetTenantId();
    }

    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _accessor;
        public TenantProvider(IHttpContextAccessor accessor) => _accessor = accessor;

        public int GetTenantId()
        {
            // מחלץ את ה-ID מה-Claim ששמנו ב-JWT
            var claim = _accessor.HttpContext?.User.FindFirst("OrganizationId")?.Value;
            return int.TryParse(claim, out int id) ? id : 0;
        }
    }
}
