using LotteryApi.Data;
using LotteryApi.Repositories;
using LotteryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LotteryApi.Middleware;
using Serilog;



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
            Description = "הכניסו את הטוקן בפורמט: Bearer {your_token}"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
        });
    });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular",
            policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });
    builder.Services.AddDbContext<LotteryDbContext>(options =>
    //options.UseSqlServer("Server=Srv2\\pupils;Database=LotteryDB;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True;"));

    options.UseSqlServer("Server=DESKTOP-PKVNNGR;Database=LotteryDB;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True;"));
    //builder.Services.AddControllers()
    //    .AddJsonOptions(options =>
    //    {
    //        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    //    });
    builder.Services.AddHttpContextAccessor();
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] ?? "YourFallbackVerySecretKey123!";
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // אימות שולח הטוקן
                ValidateAudience = true, // אימות קהל היעד
                ValidateLifetime = true, // בדיקה שהטוקן לא פג תוקף
                ValidateIssuerSigningKey = true, // אימות חתימת הטוקן
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
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

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAngular");
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
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
