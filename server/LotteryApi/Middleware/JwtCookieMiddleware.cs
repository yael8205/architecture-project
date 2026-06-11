namespace LotteryApi.Middleware
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _cookieName;

        public JwtCookieMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _cookieName = configuration["Jwt:CookieName"] ?? "access_token";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization")
                && context.Request.Cookies.TryGetValue(_cookieName, out var token)
                && !string.IsNullOrWhiteSpace(token))
            {
                context.Request.Headers.Authorization = $"Bearer {token}";
            }

            await _next(context);
        }
    }
}
