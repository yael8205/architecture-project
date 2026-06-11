using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class OrderModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string ParticipantId { get; set; } = null!;

        [ForeignKey("ParticipantId")]
        public UserModel Participant { get; set; }

        public ICollection<PackageInOrderModel> PackagesInOrder { get; set; } = new List<PackageInOrderModel>();

        [Required]
        public int SumPrice { get; set; }

        [Required]
        public DateOnly date { get; set; }

        public string OrganizationId { get; set; } = null!;
    }
}
                                                                                            