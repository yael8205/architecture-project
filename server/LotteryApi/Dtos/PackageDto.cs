using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class PackageDto
    {
        public int Id { get; set; }
      
        public string Name { get; set; }
      
        public int Price { get; set; }
       
        public int QtyClassicCards { get; set; }
      
        public int QtySpecialCards { get; set; }
     
        public int QtyPrimumCards { get; set; }
    }
    public class PackageCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int QtyClassicCards { get; set; }
        [Required]
        public int QtySpecialCards { get; set; }
        [Required]
        public int QtyPrimumCards { get; set; }
    }
    public class PackageUpdateDto
    {
        public string? Name { get; set; }
       
        public int? Price { get; set; }
        
        public int? QtyClassicCards { get; set; }
       
        public int? QtySpecialCards { get; set; }
       
        public int? QtyPrimumCards { get; set; }
    }
}
