using LotteryApi.Dtos;
using LotteryApi.Services;
using Microsoft.AspNetCore.Mvc;
using static LotteryApi.Dtos.AuthDto;

namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            var result = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            var expiryMinutes = _configuration.GetValue<int>("Jwt:ExpiryMinutes", 60);
            var cookieName = _configuration["Jwt:CookieName"] ?? "access_token";

            Response.Cookies.Append(cookieName, result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            });

            result.Token = string.Empty;

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserCreateDto createDto)
        {
            var user = await _userService.CreateUserAsync(createDto);
            return Ok(user);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var cookieName = _configuration["Jwt:CookieName"] ?? "access_token";
            Response.Cookies.Delete(cookieName);
            return NoContent();
        }
    }
}
