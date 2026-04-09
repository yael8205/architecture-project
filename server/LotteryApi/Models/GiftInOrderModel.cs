using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class GiftInOrderModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public int GiftId { get; set; }
        [ForeignKey("GiftId")]
        public GiftModel Gift { get; set; }
        [Required]
        public int PackageInOrderId { get; set; }
        [ForeignKey("PackageInOrderId")]
        public PackageInOrderModel PackageInOrder { get; set; }
        [Required]
        // public int OrderId { get; set; }
        // [ForeignKey("OrderId")]
        //  public OrderModel Order { get; set; }
        public int OrganizationId { get; set; }
        public bool IsWinner { get; set; } = false;
    }
}
