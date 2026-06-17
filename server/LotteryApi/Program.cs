using LotteryApi.Configuration;
using LotteryApi.Models;
using LotteryApi.Repositories;
using LotteryApi.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using LotteryApi.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Driver;
using Serilog;
using System.Threading.RateLimiting;



 Log.Logger = new LoggerConfiguration()
     .ReadFrom.Configuration(new ConfigurationBuilder()
         .AddJsonFile("appsettings.json")
         .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
         .Build())
     .Enrich.FromLogContext()
     .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
  .CreateLogger();

try
{
    Log.Information("Starting Store API application");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) => configuration
     .ReadFrom.Configuration(context.Configuration)
     .ReadFrom.Services(services)
     .Enrich.FromLogContext());
  

    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
   
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter: Bearer {your_jwt_token}"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
    });

    var mongoSection = builder.Configuration.GetSection("MongoDb");
    builder.Services.Configure<MongoDbSettings>(mongoSection);
    var mongoSettings = mongoSection.Get<MongoDbSettings>()
        ?? throw new InvalidOperationException("MongoDb settings are not configured.");

    builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoSettings.ConnectionString));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoSettings.DatabaseName));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<PackageModel>("Packages"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<UserModel>("Users"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<CategoryModel>("Categories"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<DonorModel>("Donors"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Organization>("Organizations"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<OrderModel>("Orders"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<ShoppingCartModel>("ShoppingCarts"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<GiftModel>("Gifts"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<GiftInCartModel>("GiftsInCart"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<GiftInOrderModel>("GiftsInOrder"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<PackageInCartModel>("PackagesInCart"));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<PackageInOrderModel>("PackagesInOrder"));

    builder.Services.AddHttpContextAccessor();
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var secretKey = jwtSection["Key"]
        ?? throw new InvalidOperationException("Jwt:Key is not configured");
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                RoleClaimType = ClaimTypes.Role
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsJsonAsync(new
                    {
                        status = 401,
                        message = "Unauthorized"
                    });
                }
            };
        });
    builder.Services.AddAuthorization();

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IPackageInCartRepository, PackageInCartRepository>();
    builder.Services.AddScoped<IPackageInCartService, PackageInCartService>();
    builder.Services.AddScoped<IPackageRepository, PackageRepository>();
    builder.Services.AddScoped<IPackageService, PackageService>();
    builder.Services.AddScoped<IPackageInOrderRepository, PackageInOrderRepository>();
    builder.Services.AddScoped<IPakageInOrderService, PakageInOrderService>();
    builder.Services.AddScoped<IGiftRepoditory, GiftRepoditory>();
    builder.Services.AddScoped<IGiftService, GiftService>();
    builder.Services.AddScoped<IGiftInCartRepository, GiftInCartRepository>();
    builder.Services.AddScoped<IGiftInCartService, GiftInCartService>();
    builder.Services.AddScoped<IGiftInOrderRepositorycs, GiftInOrderRepositorycs>();
    builder.Services.AddScoped<IGiftInOrderService, GiftInOrderService>();
    builder.Services.AddScoped<IDonorRepository, DonorRepository>();
    builder.Services.AddScoped<IDonorService, DonorService>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
    builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<ITenantProvider, TenantProvider>();
    builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
    builder.Services.AddScoped<IOrganizationService, OrganizationService>();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["Redis:Configuration"] ?? "localhost:6379";
        options.InstanceName = builder.Configuration["Redis:InstanceName"] ?? "LotteryInstance_";
    });

    builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

    // הגדרת ה-Policy
    builder.Services.AddRateLimiter(options =>
    {
        options.AddPolicy("fixed", _ => RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: "global",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 100,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
                SegmentsPerWindow = 4
            }));

        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    });
    var app = builder.Build();

    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var payload = System.Text.Json.JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
            await context.Response.WriteAsync(payload);
        });
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAngular");
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseMiddleware<JwtCookieMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
