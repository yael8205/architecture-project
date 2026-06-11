using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class PackageInOrderModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string PackageId { get; set; } = null!;

        [ForeignKey("PackageId")]
        public PackageModel Package { get; set; }

        [Required]
        public string OrderId { get; set; } = null!;

        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }

        [Required]
        public int PriceAtPurchase { get; set; }

        public string OrganizationId { get; set; } = null!;
        public ICollection<GiftInOrderModel> GiftsInPackage { get; set; } = new List<GiftInOrderModel>();
    }
}
