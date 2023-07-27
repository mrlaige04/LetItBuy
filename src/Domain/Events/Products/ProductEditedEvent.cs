namespace Domain.Events;
public class ProductEditedEvent : BaseEvent
{
    public Product Product { get; }
    public string? Property { get; }

    public ProductEditedEvent(Product product, string property)
    {
        Product = product;
        Property = property;
    }

    public ProductEditedEvent(Product product)
    {
        Product = product;
    }
}
