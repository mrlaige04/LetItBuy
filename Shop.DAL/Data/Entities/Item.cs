using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities
{
    [Table("Items")]
    public class Item
    {
        public Guid ID { get; set; }
        public ApplicationUser OwnerUser { get; set; }
        public string Name { get; set; }

        public Guid OwnerID { get; set; }
        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        public Currency Currency { get; set; }

        
        

        
        public Guid? Category_Id { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsNew { get; set; }




        public ICollection<NumberCriteriaValue>? NumberCriteriaValues { get; set; } = null!;
        public ICollection<StringCriteriaValue>? StringCriteriaValues { get; set; } = null!;

        public ICollection<ItemPhoto> Photos { get; set; } = null!;
    }

    public enum Currency
    {
        USD,
        UAH,
        EUR
    }
}
