namespace LotteryApi.Dtos
{
    public class OrganizationDto


    { 
        public record OrganizationResponseDto(
            string Id,
            string Slug,
            string Name,
            string PrimaryColor,
            string SecondaryColor,
            string AccentColor,
            string AccentContrast,
            string LogoUrl);

      
        public record OrganizationUpdateDto(
            string Name,
            string PrimaryColor,
            string SecondaryColor,
            string AccentColor,
            string AccentContrast,
            string LogoUrl);
    }
}

