using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class GiftInOrderModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string GiftId { get; set; } = null!;

        [ForeignKey("GiftId")]
        public GiftModel Gift { get; set; }

        [Required]
        public string PackageInOrderId { get; set; } = null!;

        [ForeignKey("PackageInOrderId")]
        public PackageInOrderModel PackageInOrder { get; set; }

        [Required]
        // public string OrderId { get; set; }
        // [ForeignKey("OrderId")]
        // public OrderModel Order { get; set; }
        public string OrganizationId { get; set; } = null!;

        public bool IsWinner { get; set; } = false;
    }
}
