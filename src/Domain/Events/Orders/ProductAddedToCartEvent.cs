namespace Domain.Events;
public class ProductAddedToCartEvent : BaseEvent
{
    public Product Product { get; }
    public ProductAddedToCartEvent(Product product)
    {
        Product = product;
    }
}