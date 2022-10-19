
using Shop.DAL.Data.Entities.Interfaces;

namespace Shop.DAL.Data.Entities
{
    public class NumberCharacteristic : ICharacteristic
    {
        public Guid ID { get; set; }
        public ICollection<Item> Items { get; set; } = null!;


        public NumberCriteria Criteria { get; set; }
        public Guid CriteriaID { get; set; }

        public NumberValue Value { get; set; }
        public Guid ValueID { get; set; }
        
    }
}
