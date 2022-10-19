using Shop.DAL.Data.Entities.Interfaces;
namespace Shop.DAL.Data.Entities
{
    public class StringCharacteristic : ICharacteristic
    {
        public Guid ID { get; set; }
        public ICollection<Item> Items { get; set; } = null!;

        public StringCriteria Criteria { get; set; }
        public Guid CriteriaID { get; set; }

        public StringValue Value { get; set; }
        public Guid ValueID { get; set; }
    }
}
