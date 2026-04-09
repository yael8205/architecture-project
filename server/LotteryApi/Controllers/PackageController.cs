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
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService ;
        public PackageController(IPackageService packageService)
        {
            _packageService =  packageService;
        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackageAsync()
        {
            var packages = await _packageService.GetPackageAsync();
            return Ok(packages);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<PackageDto>> GetPackageByIdAsync(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null)
            {
                return NotFound(new { message = $"Package with ID {id} not found." });
            }
            return Ok(package);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult<PackageDto>> CreatePackageAsync([FromBody] PackageCreateDto package)
        {
            var newPackage = await _packageService.CreatePackageAsync(package);
            return Ok(newPackage);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<ActionResult<PackageDto>> UpdatePackageAsync(int id, [FromBody] PackageUpdateDto package)
        {
            var updatePackage = await _packageService.UpdatePackageAsync(id, package);
            if (updatePackage == null)
            {
                return NotFound(new { message = $"Package with ID {id} not found." });
            }
            return Ok(updatePackage);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePackageAsync(int id)
        {
            var isDeleted = await _packageService.DeletePackageAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"Package with ID {id} not found." });
            }
            return NoContent();
        }

    }
}
