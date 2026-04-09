using LotteryApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryApi.Dtos
{
    public class PackageInOrderDto
    {
        public int Id { get; set; }

        public int PackageId { get; set; }

        public int PriceAtPurchase { get; set; }
        public string PackageName { get; set; }
        public ICollection<GiftInOrderDto> GiftsInPackage { get; set; } = new List<GiftInOrderDto>();
    }
}
