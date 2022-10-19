namespace Shop.DAL.Data.Entities.Interfaces
{
    public interface ICharacteristic
    {
        public Guid ID { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
