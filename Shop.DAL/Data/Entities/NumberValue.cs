using Shop.DAL.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("NumberValues")]
    public class NumberValue : IValue
    {
        public NumberValue()
        { 
            ValueID = Guid.NewGuid();
        }
        public NumberValue(double val) : this()
        {
            Value = val;
        }


        
        public Guid ValueID { get ; set ; }
        [Key]
        public double Value { get; set; }


        
        public ICollection<NumberCriteria>? numberCriterias { get; set; }



        
        public override bool Equals(object? obj)
        {
            if (obj is NumberValue value) return value.Value == Value;
            return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator NumberValue(double value)
        {
            return new NumberValue(value)
            {
                ValueID = Guid.NewGuid()
            };
        }

       
    }
}
