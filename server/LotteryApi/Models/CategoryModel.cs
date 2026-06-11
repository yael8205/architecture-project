using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class CategoryModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string Name { get; set; }

        public string OrganizationId { get; set; } = null!;

        public ICollection<GiftModel> Gifts { get; set; } = new List<GiftModel>();
    }
}
