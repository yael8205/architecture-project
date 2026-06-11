using LotteryApi.Dtos;
using LotteryApi.Models;
using LotteryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        //private readonly ILogger<UsersController> _logger;

        public UserController(IUserService userService)
           // ILogger<UsersController> logger)
        {
            _userService = userService;
           // _logger = logger;
        }
       
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return Ok(user);
        }

        [HttpPost]
      
        public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateDto createDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createDto);
                return CreatedAtAction(nameof(GetUserByIdAsync), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UserUpdateDto updateDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, updateDto);

                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
