using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class DonorModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required,Phone]
        public string Phone { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        public int OrganizationId { get; set; }
        public ICollection<GiftModel> Gifts { get; set; } = new List<GiftModel>();
      
    }
}
