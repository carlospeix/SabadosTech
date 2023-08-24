namespace Model1;

public class Order
{
    public Order()
    {
        Items = new List<OrderItem>();
    }

    public int Id { get; private set; }
    public DateTime CreatedOn { get; set; }
    public Customer Customer { get; set; }

    public virtual List<OrderItem> Items { get; private set; }
}
