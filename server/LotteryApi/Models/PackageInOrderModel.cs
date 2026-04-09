using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class PackageInOrderModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public int PackageId { get; set; }
        [ForeignKey("PackageId")]
        public PackageModel Package { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }
        [Required]
        public int PriceAtPurchase { get; set; }
        public int OrganizationId { get; set; }
        public ICollection<GiftInOrderModel> GiftsInPackage { get; set; } = new List<GiftInOrderModel>();
    }
}
