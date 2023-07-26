namespace Domain.Events;
public class ProductDelivered : BaseEvent
{
    public Product Product { get; }
    public ProductDelivered(Product product)
    {
        Product = product;
    }
}