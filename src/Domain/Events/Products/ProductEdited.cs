namespace Domain.Events;
public class ProductEdited : BaseEvent
{
    public Product Product { get; }
    public ProductEdited(Product product)
    {
        Product = product;
    }
}
