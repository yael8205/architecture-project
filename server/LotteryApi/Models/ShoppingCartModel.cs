using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class ShoppingCartModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public UserModel Participant { get; set; }
        public ICollection<PackageInCartModel> PackagesInShoppingCart { get; set; } = new List<PackageInCartModel>();
        public ICollection<GiftInCartModel> GiftsInShoppingCart { get; set; } = new List<GiftInCartModel>();

        [Required]
        public int SumPrice { get; set; }= 0;
        public int OrganizationId { get; set; }
    }
}
