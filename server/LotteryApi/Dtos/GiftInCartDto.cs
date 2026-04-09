using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class GiftInCartDto
    {
        public int Id { get; set; }      
        public int GiftId { get; set; }    
        public string GiftName { get; set; }
        public string giftPictureUrl { get; set; }
        public string giftCardPrice { get; set; }
        public int Qty { get; set; } = 1;
    }
    public class GiftInCartCreateDto
    {
        [Required]
        public int PackageInCartId { get; set; }
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int Qty { get; set; } = 1;
    }
    public class GiftInCartUpdateDto
    {
      
        public int Qty { get; set; }
    }
}
