using Shop.DAL.Data.Entities.Interfaces;

namespace Shop.DAL.Data.Entities
{
    public class NumberCriteria : ICriteria
    {
        public Guid ID { get ; set; }
        public string Name { get ; set ; }
        public ICollection<Category>? Categories { get; set; }

        public IEnumerable<NumberValue>? DefaultValues { get; set; }

        public ICollection<Characteristic>? Characteristics { get; set; }
    }
}
