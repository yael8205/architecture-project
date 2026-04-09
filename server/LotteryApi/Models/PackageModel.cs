using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class PackageModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int QtyClassicCards { get; set; }
        [Required]
        public int QtySpecialCards { get; set; }
        [Required]
        public int QtyPrimumCards { get; set; }
        public int OrganizationId { get; set; }

    }
}
