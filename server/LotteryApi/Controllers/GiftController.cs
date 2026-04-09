using LotteryApi.Dtos;
using LotteryApi.Enums;
using LotteryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;
        public GiftController(IGiftService giftService)
        {
            _giftService = giftService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiftDto>>> GetGiftAsync()
        {
            var gifts = await _giftService.GetGiftsAsync();
            return Ok(gifts);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GiftDto>> GetGiftByIdAsync(int id)
        {
            var gift = await _giftService.GetGiftByIdAsync(id);
            if (gift == null)
            {
                return NotFound(new { message = $"Gift with ID {id} not found." });
            }
            return Ok(gift);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult<GiftDto>> CreateGiftAsync([FromBody] GiftCreateDto gift)
        {
            var newGift = await _giftService.CreateGiftAsync(gift);
            return Ok(newGift);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<ActionResult<GiftDto>> UpdatePackageAsync(int id, [FromBody] GiftUpdateDto gift)
        {
            var updateGift = await _giftService.UpdateGiftAsync(id, gift);
            if (updateGift == null)
            {
                return NotFound(new { message = $"Gift with ID {id} not found." });
            }
            return Ok(updateGift);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGiftAsync(int id)
        {
            var isDeleted = await _giftService.DeleteGiftAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"Gift with ID {id} not found." });
            }
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> SearchGifts(
    [FromQuery] string? giftName,
  [FromQuery] string? donorName,
  [FromQuery] int? minPurchasers)
        {
            var results = await _giftService.SearchGiftsAsync(giftName, donorName, minPurchasers);

            if (results == null || !results.Any())
            {
                return NotFound("לא נמצאו מתנות התואמות לחיפוש.");
            }

            return Ok(results);
        }
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> FilterGifts(
  [FromQuery] int? categoryId,
  [FromQuery] CardPriceEnum? priceType)
        {
            var results = await _giftService.FilteredGiftsAsync(categoryId, priceType);

            if (results == null || !results.Any())
            {
                return NotFound("לא נמצאו מתנות התואמות לחיפוש😫😪");
            }

            return Ok(results);
        }
        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> SortedGiftsExpensiveAsync([FromQuery] string sortBy)
        {
            var results = await _giftService.SortedGiftsExpensiveAsync(sortBy);

            if (results == null || !results.Any())
            {
                return NotFound("לא נמצאו מתנות התואמות לחיפוש😫😪");
            }

            return Ok(results);
        }
    }
}
