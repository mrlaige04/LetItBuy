using Shop.DAL.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("StringValues")]
    public class StringValue : IValue
    {
        public StringValue()
        {
            ValueID = Guid.NewGuid();
        }
        public StringValue(string val) : this()
        {
            Value = val;
        }
        [Key]
        public Guid ValueID { get ; set ; }
        [Key]
        public string Value { get; set; }
        public ICollection<StringCriteria>? stringCriterias { get; set; }

        public static implicit operator StringValue(string value)
        {
            return new StringValue()
            {
                Value = value,
                ValueID = Guid.NewGuid()
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is StringValue value) return value.Value.ToLower().Trim().Replace(" ","") == 
                    Value.ToLower().Trim().Replace(" ", "");
            return false;
        }

        public override int GetHashCode()
        {
            return Value.ToLower().Trim().Replace(" ", "").GetHashCode();
        }
    }
}
