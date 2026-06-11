using LotteryApi.Enums;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class GiftModel : ITenantEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public string CategoryId { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }

        public int PrizeQuantity { get; set; } = 1;

        [Required]
        public CardPriceEnum CardPrice { get; set; }

        public string? PictureUrl { get; set; }

        [Required]
        public string DonorId { get; set; } = null!;

        [ForeignKey("DonorId")]
        public DonorModel Donor { get; set; }

        public string OrganizationId { get; set; } = null!;
        public ICollection<GiftInOrderModel> GifPurchased { get; set; } = new List<GiftInOrderModel>();
    }
}