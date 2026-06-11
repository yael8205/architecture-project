using LotteryApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class PackageInCartDto
    {
        public string Id { get; set; } = null!;

        public string PackageId { get; set; } = null!;
        
        public string PackageName { get; set; }
        public int PackagePrice { get; set; }
    
        public ICollection<GiftInCartDto> GiftsInPackage { get; set; } = new List<GiftInCartDto>();

    }
    public class PackageInCartCreateDto
    {
        [Required]
        public string PackageId { get; set; } = null!;
    }
}
