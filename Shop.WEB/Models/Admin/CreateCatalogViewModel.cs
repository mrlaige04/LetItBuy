using Shop.DAL.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shop.WEB.Models
{
    public class CreateCatalogViewModel
    {
        public Category? Parent { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Characteristic>? Charatrestics { get; set; }
    }
}
