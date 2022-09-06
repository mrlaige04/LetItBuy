using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Catalogs")]
    public class Catalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Catalog? Parent { get; set; }
        public List<Charatrestic> Charatrestics { get; set; } = new();
    }
}
