using Shop.DAL.Data.Entities.Interfaces;

namespace Shop.DAL.Data.Entities
{
    public class NumberCriteria : ICriteria
    {
        public Guid ID { get ; set; }
        public string Name { get ; set ; }
        public NumberCriteria()
        {
            ID = Guid.NewGuid();
        }
        public ICollection<Category>? Categories { get; set; }

        public ICollection<NumberValue>? DefaultValues { get; set; }

        public ICollection<NumberCharacteristic>? Characteristics { get; set; }
    }
}
