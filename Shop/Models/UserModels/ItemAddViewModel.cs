using Shop.DAL.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models.UserModels
{
    public class ItemAddViewModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(0, 999999999)]
        public decimal Price { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public Guid CategoryID { get; set; }
        [Required]
        public bool IsNew { get; set; }


    }
}
