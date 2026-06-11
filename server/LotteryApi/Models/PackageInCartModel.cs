using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class PackageInCartModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string PackageId { get; set; } = null!;

        [ForeignKey("PackageId")]
        public PackageModel Package { get; set; }

        [Required]
        public string CartId { get; set; } = null!;

        [ForeignKey("CartId")]
        public ShoppingCartModel ShoppingCart { get; set; }

        public string OrganizationId { get; set; } = null!;
        public ICollection<GiftInCartModel> GiftsInPackage { get; set; } = new List<GiftInCartModel>();
    }
}
