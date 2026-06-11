using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class OrderDto
    {
       
            public string Id { get; set; } = null!;
            public string ParticipantId { get; set; } = null!;    
            public string ParticipantName { get; set; }
            public ICollection<PackageInOrderDto> PackagesInOrder { get; set; } = new List<PackageInOrderDto>();
            public int SumPrice { get; set; }
            public DateOnly date { get; set; }
        
    }
    public class OrderCreateDto
    {
        [Required]
        public string ParticipantId { get; set; } = null!;
        [Required]
        public ICollection<PackageInOrderDto> PackagesInOrder { get; set; } = new List<PackageInOrderDto>();
        [Required]
        public int SumPrice { get; set; }
        [Required]
        public DateOnly date { get; set; }

    }
}
