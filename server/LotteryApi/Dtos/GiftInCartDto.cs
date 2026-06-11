using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class GiftInCartDto
    {
        public string Id { get; set; } = null!;      
        public string GiftId { get; set; } = null!;    
        public string GiftName { get; set; }
        public string giftPictureUrl { get; set; }
        public string giftCardPrice { get; set; }
        public int Qty { get; set; } = 1;
    }
    public class GiftInCartCreateDto
    {
        [Required]
        public string PackageInCartId { get; set; } = null!;
        [Required]
        public string GiftId { get; set; } = null!;
        [Required]
        public int Qty { get; set; } = 1;
    }
    public class GiftInCartUpdateDto
    {
      
        public int Qty { get; set; }
    }
}
