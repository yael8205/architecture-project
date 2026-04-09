using LotteryApi.Enums;
using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class GiftDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
       public string CategoryName { get; set; }
        public int PrizeQuantity { get; set; } = 1;
        public string CardPrice { get; set; }
        public string? PictureUrl { get; set; }
        public int DonorId { get; set; }
        public string DonorName { get; set; }
        public ICollection<GiftPurchaserDto> GifPurchased { get; set; } = new List<GiftPurchaserDto>();

        public int PurchasersCount { get; set; }

    }
    public class GiftCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int PrizeQuantity { get; set; } = 1;
        [Required]
        public CardPriceEnum CardPrice { get; set; }
        public string? PictureUrl { get; set; }
        [Required]
        public int DonorId { get; set; }
       
    }
    public class GiftUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? PrizeQuantity { get; set; }
        public CardPriceEnum? CardPrice { get; set; }
        public string? PictureUrl { get; set; }
        public int? DonorId { get; set; }
       
    }

}
