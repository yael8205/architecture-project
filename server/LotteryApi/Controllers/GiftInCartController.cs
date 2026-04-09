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
    public class GiftInCartController : ControllerBase
    {
        private readonly IGiftInCartService _giftInCartService ;
        public GiftInCartController(IGiftInCartService giftInCartService)
        {
            _giftInCartService =  giftInCartService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GiftInCartDto>> GetGiftInCartByIdAsync(int id)
        {
            var giftInCart = await _giftInCartService.GetGiftInCartByIdAsync(id);
            if (giftInCart == null)
            {
                return NotFound(new { message = $"GiftInCart with ID {id} not found." });
            }
            return Ok(giftInCart);
        }
        [HttpGet("package/{packageInCartId}/gift/{giftId}")]
        public async Task<ActionResult<GiftInCartDto>> GetGiftInCartByIdAndByPackageAsync(int giftId, int packageInCartId)
        {
            var giftInCart = await _giftInCartService.GetGiftInCartByIdAndByPackageAsync(giftId, packageInCartId);
            if (giftInCart == null)
            {
                return NotFound(new { message = $"The gift ID :{giftId} in package ID: {packageInCartId} not found." });
            }
            return Ok(giftInCart);
        }
        [HttpPost]
        public async Task<ActionResult<GiftInCartDto>> CreateOrUpdateGiftInCartAsync([FromBody] GiftInCartCreateDto giftInCart)
        {
            var newGiftInCart = await _giftInCartService.CreateOrUpdateGiftInCartAsync(giftInCart);
            return Ok(newGiftInCart);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePackageInCartAsync(int id)
        {
            var isDeleted = await _giftInCartService.DeleteGiftInCartAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"GiftInCart with ID {id} not found." });
            }
            return NoContent();
        }
    }
}
