using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
    }
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
    public class CategoryUpdateDto
    {
     
        public string? Name { get; set; }
    }

}
