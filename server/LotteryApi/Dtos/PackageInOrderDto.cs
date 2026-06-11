using LotteryApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Dtos
{
    public class PackageInOrderDto
    {
        public string Id { get; set; } = null!;

        public string PackageId { get; set; } = null!;

        public int PriceAtPurchase { get; set; }
        public string PackageName { get; set; }
        public ICollection<GiftInOrderDto> GiftsInPackage { get; set; } = new List<GiftInOrderDto>();
    }
}
