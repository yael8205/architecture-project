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
    public class PackageInOrderController : ControllerBase
    {
        private readonly IPakageInOrderService _pakageInOrderService;
        public PackageInOrderController(IPakageInOrderService pakageInOrderService)
        {
            _pakageInOrderService = pakageInOrderService;
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<PackageInOrderDto>> GetPackageInOrderByIdAsync(int id)
        {
            var packageInOrder = await _pakageInOrderService.GetPackageInOrderByIdAsync(id);
            if (packageInOrder == null)
            {
                return NotFound(new { message = $"PackageInOrder with ID {id} not found." });
            }
            return Ok(packageInOrder);
        }
        //[HttpPost]
        //public async Task<ActionResult<PackageInOrderDto>> CreatePackageInCartAsync([FromBody] PackageInOrderCreateDto packageInOrder)
        //{
        //    var newPackageInOrder = await _pakageInOrderService.CreatePackageInOrderAsync(packageInOrder);
        //    return Ok(newPackageInOrder);
        //}

    }
}
