using LotteryApi.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Models
{
    public class UserModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string Name { get; set; }

        [Required, MinLength(6), MaxLength(20)]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public UserRoleEnum Role { get; set; } = UserRoleEnum.Participant;

        public string OrganizationId { get; set; } = null!;

        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
