using LotteryApi.Dtos;
using LotteryApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using static LotteryApi.Dtos.AuthDto;

namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        // private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService)
           // ILogger<AuthController> logger)
    {
            _userService = userService;
            //  _logger = logger;
        }


        [HttpPost("login")]

        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {

            var result = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            

            return Ok(result);
        }

        
        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register([FromBody] UserCreateDto createDto)
        {
           
                var user = await _userService.CreateUserAsync(createDto);
                return Ok(user);
          

        }
    }
}
