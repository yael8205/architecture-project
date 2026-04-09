using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
     
        public int ParticipantId { get; set; }
      
        public string ParticipantName { get; set; }
        public ICollection<PackageInCartDto> PackagesInShoppingCart { get; set; } = new List<PackageInCartDto>();
        public int SumPrice { get; set; }
    }
    public class ShoppingCartCreateDto
    {
        [Required]
        public int ParticipantId { get; set; }
       
      
    }
  
}
