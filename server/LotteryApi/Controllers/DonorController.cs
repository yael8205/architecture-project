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
    [Authorize(Roles = "Manager")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService ;
         public DonorController(IDonorService donorService)
        {
            _donorService =  donorService;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<DonorDto>>> GetDonorsAsync()
        {
            var donors = await _donorService.GetDonorsAsync();
            return Ok(donors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DonorDto>> GetDonorsByIdAsync(int id)
        {
            var donor = await _donorService.GetDonorsByIdAsync(id);
            if (donor == null)
            {
                return NotFound(new { message = $"Donor with ID {id} not found." });
            }
            return Ok(donor);
        }
        [HttpPost]
        public async Task<ActionResult<DonorDto>> CreateDonorsAsync([FromBody] DonorCreateDto donor)
        {
            var createDonor=await _donorService.CreateDonorsAsync(donor);
                return Ok(createDonor);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DonorDto>> UpdateDonorsAsync(int id, [FromBody] DonorUpdateDto updateDonor)
        {
            var updatedDonor = await _donorService.UpdateDonorsAsync(id, updateDonor);
            if (updatedDonor == null)
            {
                return NotFound(new { message = $"Donor with ID {id} not found." });
            }
            return Ok(updatedDonor);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDonorsAsync(int id)
        {
            var isDeleted = await _donorService.DeleteDonorsAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"Donor with ID {id} not found." });
            }
            return NoContent();
        }
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<DonorDto>>> GetFilteredDonors(
        [FromQuery] string? name,
        [FromQuery] string? email,
        [FromQuery] string? giftName)
        {
            var results = await _donorService.GetFilteredDonorsAsync(name, email, giftName);
            return Ok(results);
        }
    }
}
