using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class PackageInCartModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public int PackageId { get; set; }
        [ForeignKey("PackageId")]
        public PackageModel Package { get; set; }
        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public ShoppingCartModel ShoppingCart { get; set; }

        public int OrganizationId { get; set; }
        public ICollection<GiftInCartModel> GiftsInPackage { get; set; } = new List<GiftInCartModel>();

    }
}
