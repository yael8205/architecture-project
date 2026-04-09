using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class GiftInCartModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required] 
        public int GiftId { get; set; }
        [ForeignKey("GiftId")]
        public GiftModel Gift { get; set; }
        [Required]
        public int PackageInCartId { get; set; }
        [ForeignKey("PackageInCartId")]
        public PackageInCartModel PackageInCart { get; set; }
        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public ShoppingCartModel ShoppingCart { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        public int Qty { get; set; } = 1;
        
    }
}
