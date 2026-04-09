using LotteryApi.Exceptions;
using LotteryApi.Models;
using LotteryApi.Repositories;
using static LotteryApi.Dtos.OrganizationDto;

namespace LotteryApi.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        public OrganizationService(IOrganizationRepository organizationRepository) => _organizationRepository = organizationRepository;

        public async Task<IEnumerable<OrganizationResponseDto>> GetAllAsync()
        {
            var orgs = await _organizationRepository.GetAllAsync();
            return orgs.Select(MapToResponse);
        }

        public async Task<OrganizationResponseDto> GetBySlugAsync(string slug)
        {
            var org = await _organizationRepository.GetBySlugAsync(slug) ?? throw new NotFoundException("ארגון לא נמצא");
            return MapToResponse(org);
        }

        public async Task CreateAsync(Organization org) // מיועד למנהל על/Seed
        {
            if (await _organizationRepository.GetBySlugAsync(org.Slug) != null)
                throw new ConflictException("ה-Slug הזה כבר בשימוש");

            await _organizationRepository.AddAsync(org);
            await _organizationRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, OrganizationUpdateDto dto)
        {
            var org = await _organizationRepository.GetByIdAsync(id) ?? throw new NotFoundException("ארגון לא נמצא");

            org.Name = dto.Name;
            org.PrimaryColor = dto.PrimaryColor;
            org.SecondaryColor = dto.SecondaryColor;
            org.AccentColor = dto.AccentColor;
            org.AccentContrast = dto.AccentContrast;
            org.LogoUrl = dto.LogoUrl;

            _organizationRepository.Update(org);
            await _organizationRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var org = await _organizationRepository.GetByIdAsync(id) ?? throw new NotFoundException("ארגון לא נמצא");
            _organizationRepository.Delete(org);
            await _organizationRepository.SaveChangesAsync();
        }

        private OrganizationResponseDto MapToResponse(Organization o) =>
            new(o.Id, o.Slug, o.Name, o.PrimaryColor, o.SecondaryColor, o.AccentColor, o.AccentContrast, o.LogoUrl);
    }
}

