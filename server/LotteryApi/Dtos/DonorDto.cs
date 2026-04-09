using LotteryApi.Models;
using System.ComponentModel.DataAnnotations;

namespace LotteryApi.Dtos
{
    public class DonorDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
       
        public string Email { get; set; } = string.Empty;
        public List<string> GiftNames { get; set; } = new List<string>();
    }
    public class DonorCreateDto
    {

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, Phone]
        public string Phone { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
    public class DonorUpdateDto
    {
       
        public string? Name { get; set; }
        [ Phone]
        public string? Phone { get; set; }
        [ EmailAddress]
        public string? Email { get; set; }
    }

}
