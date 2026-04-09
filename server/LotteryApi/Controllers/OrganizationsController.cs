using LotteryApi.Services;
using Microsoft.AspNetCore.Mvc;
using static LotteryApi.Dtos.OrganizationDto;

namespace LotteryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _service;
        public OrganizationsController(IOrganizationService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{slug}")]
        public async Task<IActionResult> Get(string slug) => Ok(await _service.GetBySlugAsync(slug));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrganizationUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
