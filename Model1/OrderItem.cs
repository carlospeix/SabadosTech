namespace Model1;

public class OrderItem
{
    private OrderItem() { }

    public OrderItem(Order order, Product product, int quantity = 1)
    {
        Order = order;
        Product = product;
        Quantity = quantity;
    }

    public int Id { get; private set; }

    public virtual Order Order { get; private set; }
    public virtual Product Product { get; private set; }
    public virtual int Quantity { get; private set; }
}
