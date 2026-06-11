using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class GiftInCartModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string GiftId { get; set; } = null!;

        [ForeignKey("GiftId")]
        public GiftModel Gift { get; set; }

        [Required]
        public string PackageInCartId { get; set; } = null!;

        [ForeignKey("PackageInCartId")]
        public PackageInCartModel PackageInCart { get; set; }

        [Required]
        public string CartId { get; set; } = null!;

        [ForeignKey("CartId")]
        public ShoppingCartModel ShoppingCart { get; set; }

        [Required]
        public string OrganizationId { get; set; } = null!;

        public int Qty { get; set; } = 1;
    }
}
