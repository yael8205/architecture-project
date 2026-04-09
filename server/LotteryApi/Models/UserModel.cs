using LotteryApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class UserModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required,MinLength(6),MaxLength(20)]
        public string Password { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,Phone]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public UserRoleEnum Role { get; set; } = UserRoleEnum.Participant;
        public int OrganizationId { get; set; }
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
