namespace Domain.Events;
public class ProductAddedToCart : BaseEvent
{
    public Product Product { get; }
    public ProductAddedToCart(Product product)
    {
        Product = product;
    }
}