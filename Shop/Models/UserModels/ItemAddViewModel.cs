using System.ComponentModel.DataAnnotations;

namespace Shop.Models.UserModels
{
    public class ItemAddViewModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public string CategoryID { get; set; }
    }
}
