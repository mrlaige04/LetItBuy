namespace Domain.Events;
public class ProductDeliveredEvent : BaseEvent
{
    public Product Product { get; }
    public ProductDeliveredEvent(Product product)
    {
        Product = product;
    }
}