using LotteryApi.Enums;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Models
{
    public class GiftModel: ITenantEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
        public int PrizeQuantity { get; set; } = 1;
        [Required]
        public CardPriceEnum CardPrice { get; set; }
        public string? PictureUrl { get; set; }
        [Required]
        public int DonorId { get; set; }
        [ForeignKey("DonorId")]
        public DonorModel Donor { get; set; }
        public int OrganizationId { get; set; }
        public ICollection<GiftInOrderModel> GifPurchased { get; set; } = new List<GiftInOrderModel>();


    }
}