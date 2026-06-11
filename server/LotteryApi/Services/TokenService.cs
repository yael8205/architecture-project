using LotteryApi.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LotteryApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email, string name, UserRoleEnum role, string organizationId);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string userId, string email, string name, UserRoleEnum role, string organizationId)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var secretKey = jwtSection["Key"]
                ?? throw new InvalidOperationException("JWT Key is not configured in Jwt:Key");
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expiryMinutes = int.Parse(jwtSection["ExpiryMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roleClaim = MapRoleToClaim(role);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Name, name),
                new Claim(ClaimTypes.Role, roleClaim),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("OrganizationId", organizationId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string MapRoleToClaim(UserRoleEnum role) =>
            role switch
            {
                UserRoleEnum.Manager => "Admin",
                _ => role.ToString()
            };
    }
}
