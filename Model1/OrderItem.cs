namespace Model1;

public class OrderItem
{
    private OrderItem() { }

    public OrderItem(Order order, Product product, int quantity)
    {
        Order = order;
        Product = product;
        Quantity = quantity;
        ProductPriceWhenOrdered = product.Price;
    }

    public int Id { get; private set; }

    public virtual Order Order { get; private set; }
    public virtual Product Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal ProductPriceWhenOrdered { get; private set; }
    public decimal Total => Quantity * ProductPriceWhenOrdered;

    internal void IncreaseQuantity() => ChangeQuantityBy(1);
    internal void DecreaseQuantity() => ChangeQuantityBy(-1);
    private void ChangeQuantityBy(int quantityChangeBy)
    {
        Quantity += quantityChangeBy;
    }
}
