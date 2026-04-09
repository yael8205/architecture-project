namespace LotteryApi.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public required string Slug { get; set; } 
        public required string Name { get; set; }
        public string PrimaryColor { get; set; } = "#F26522";
        public string SecondaryColor { get; set; } = "#F8F9FA";
        public string AccentColor { get; set; } = "#1E3A8A";
        public string AccentContrast { get; set; } = "#FFFFFF";
        public string LogoUrl { get; set; } = string.Empty;
    }
}
