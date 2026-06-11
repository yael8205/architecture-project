using LotteryApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Dtos
{
    public class GiftInOrderDto
    {
        public string Id { get; set; } = null!;
     
        public string GiftId { get; set; } = null!;

        public string GiftName { get; set; }
        public string GiftPictureUrl { get; set; }
        public string GiftCardPrice { get; set; }
        //public int PriceAtPurchase { get; set; }
        public bool IsWinner { get; set; } = false;
    }
   

}
