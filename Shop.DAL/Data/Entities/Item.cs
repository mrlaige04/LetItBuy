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
        public Guid ItemId { get; set; }
        public User OwnerUser { get; set; }
        public string ItemName { get; set; }

        public Guid OwnerID { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public decimal ItemPrice { get; set; }
        public Currency Currency { get; set; }

        public Guid? Category_ID { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsNew { get; set; }

        public ICollection<Characteristic> Characteristics { get; set; }


        //public string this[string criteriaName]
        //{
        //    get => Characteristics.Where(c => c.CriteriaName == criteriaName).FirstOrDefault().ValueId;
        //}
    }

    public enum Currency
    {
        USD,
        UAH,
        EUR
    }
}
