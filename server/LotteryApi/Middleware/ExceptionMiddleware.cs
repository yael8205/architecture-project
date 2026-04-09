using Serilog;
using System.Net;
using LotteryApi.Exceptions;
namespace LotteryApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger; 

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionMiddleware> logger)
        {
            _next = next; 
            _env = env;
            _logger = logger;
          
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // ניסיון להריץ את הבקשה כרגיל
            }
            catch (Exception ex)
            {
                // 1. זיהוי החתימה וקביעת סטטוס ה-HTTP
                var statusCode = ex switch
                {
                    NotFoundException => HttpStatusCode.NotFound,      // 404
                    BadRequestException => HttpStatusCode.BadRequest,  // 400
                    ConflictException => HttpStatusCode.Conflict,
                    UnauthorizedException => HttpStatusCode.Unauthorized, // הוסיפי את השורה הזו!// 409
                    _ => HttpStatusCode.InternalServerError            // 500 (קריסת שרת)
                };

                // 2. רישום לוג ב-Serilog
                if (statusCode == HttpStatusCode.InternalServerError)
                    _logger.LogError(ex, "קריסת שרת לא צפויה בנתיב: {Path}", context.Request.Path);
                else
                    _logger.LogWarning("שגיאה עסקית ({Status}): {Message} בנתיב {Path}",
                                statusCode, ex.Message, context.Request.Path);

                // 3. החזרת תשובה מסודרת ללקוח (Angular)
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var response = new
                {
                    status = context.Response.StatusCode,
                    message = ex.Message,
                    // מציג פרטי שגיאה טכניים רק למפתח, לא למשתמש הקצה
                    details = _env.IsDevelopment() ? ex.StackTrace : null
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
