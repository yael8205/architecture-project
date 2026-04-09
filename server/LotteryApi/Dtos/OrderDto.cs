using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class OrderDto
    {
       
            public int Id { get; set; }
            public int ParticipantId { get; set; }    
            public string ParticipantName { get; set; }
            public ICollection<PackageInOrderDto> PackagesInOrder { get; set; } = new List<PackageInOrderDto>();
            public int SumPrice { get; set; }
            public DateOnly date { get; set; }
        
    }
    public class OrderCreateDto
    {
        [Required]
        public int ParticipantId { get; set; }
        [Required]
        public ICollection<PackageInOrderDto> PackagesInOrder { get; set; } = new List<PackageInOrderDto>();
        [Required]
        public int SumPrice { get; set; }
        [Required]
        public DateOnly date { get; set; }

    }
}
