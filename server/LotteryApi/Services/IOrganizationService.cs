using LotteryApi.Dtos;
using LotteryApi.Models;

namespace LotteryApi.Services
{
    public interface IOrganizationService
    {
        Task CreateAsync(Organization org);
        Task DeleteAsync(int id);
        Task<IEnumerable<OrganizationDto.OrganizationResponseDto>> GetAllAsync();
        Task<OrganizationDto.OrganizationResponseDto> GetBySlugAsync(string slug);
        Task UpdateAsync(int id, OrganizationDto.OrganizationUpdateDto dto);
    }
}