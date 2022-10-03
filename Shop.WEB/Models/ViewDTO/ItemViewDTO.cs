using Shop.DAL.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shop.WEB.Models
{
    public class ItemViewDTO
    {
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        [Range(0, long.MaxValue)]
        public decimal ItemPrice { get; set; }

        public string? ImageURL { get; set; }
        public string? Description { get; set; }

        public Currency Currency { get; set; }
    }
}
