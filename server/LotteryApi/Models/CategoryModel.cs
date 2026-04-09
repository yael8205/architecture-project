using System.ComponentModel.DataAnnotations;


namespace LotteryApi.Models
{
    public class CategoryModel : ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        public ICollection<GiftModel> Gifts { get; set; } = new List<GiftModel>();

    }
}
