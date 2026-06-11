using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class PackageModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

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

        public string OrganizationId { get; set; } = null!;
    }
}
