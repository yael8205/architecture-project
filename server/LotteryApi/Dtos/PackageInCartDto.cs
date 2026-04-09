using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class PackageInCartDto
    {
        public int Id { get; set; }

        public int PackageId { get; set; }
        
        public string PackageName { get; set; }
        public int PackagePrice { get; set; }
    
        public ICollection<GiftInCartDto> GiftsInPackage { get; set; } = new List<GiftInCartDto>();

    }
    public class PackageInCartCreateDto
    {
        [Required]
        public int PackageId { get; set; }
    }
}
