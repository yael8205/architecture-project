using LotteryApi.Dtos;
using LotteryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PackageInCartController : ControllerBase
    {
        private readonly IPackageInCartService _packageInCartService ;
        public PackageInCartController(IPackageInCartService packageInCartService)
        {
            _packageInCartService =  packageInCartService;
        }
     
        [HttpGet("{id}")]
        public async Task<ActionResult<PackageInCartDto>> GetPackageInCartByIdAsync(int id)
        {
            var packageInCart = await _packageInCartService.GetPackageInCartByIdAsync(id);
            if (packageInCart == null)
            {
                return NotFound(new { message = $"PackageInCart with ID {id} not found." });
            }
            return Ok(packageInCart);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PackageInCartDto>> CreatePackageInCartAsync([FromBody] PackageInCartCreateDto packageInCart)
        {
            var newPackageInCart = await _packageInCartService.CreatePackageInCartAsync(packageInCart);
            return Ok(newPackageInCart);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePackageInCartAsync(int id)
        {
            var isDeleted = await _packageInCartService.DeletePackageInCartAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"PackageInCart with ID {id} not found." });
            }
            return NoContent();
        }
    }
}
