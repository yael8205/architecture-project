using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class ShoppingCartModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string ParticipantId { get; set; } = null!;

        [ForeignKey("ParticipantId")]
        public UserModel Participant { get; set; }

        public ICollection<PackageInCartModel> PackagesInShoppingCart { get; set; } = new List<PackageInCartModel>();
        public ICollection<GiftInCartModel> GiftsInShoppingCart { get; set; } = new List<GiftInCartModel>();

        [Required]
        public int SumPrice { get; set; } = 0;
        public string OrganizationId { get; set; } = null!;
    }
}
