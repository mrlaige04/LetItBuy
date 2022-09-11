using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class CreateCatalogViewModel
    {
        public Catalog? Parent { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Charatrestic>? Charatrestics { get; set; }
    }
}
