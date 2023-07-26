namespace Domain.Events;
public class ProductOrdered : BaseEvent
{
    public Product Product { get; }
    public ProductOrdered(Product product)
    {
        Product = product;
    }
}
