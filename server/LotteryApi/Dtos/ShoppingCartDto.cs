using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class ShoppingCartDto
    {
        public string Id { get; set; } = null!;
     
        public string ParticipantId { get; set; } = null!;
      
        public string ParticipantName { get; set; }
        public ICollection<PackageInCartDto> PackagesInShoppingCart { get; set; } = new List<PackageInCartDto>();
        public int SumPrice { get; set; }
    }
    public class ShoppingCartCreateDto
    {
        [Required]
        public string ParticipantId { get; set; } = null!;
       
      
    }
  
}
