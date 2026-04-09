using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class OrderModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public UserModel Participant { get; set; }
        public ICollection<PackageInOrderModel> PackagesInOrder { get; set; } = new List<PackageInOrderModel>();

        [Required]
        public int SumPrice { get; set; }
        [Required]
        public DateOnly date {  get; set; }
        public int OrganizationId { get; set; }
    }
}
                                                                                            