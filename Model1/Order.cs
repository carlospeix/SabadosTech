namespace Model1;

public class Order
{
    private Order() { }

    public Order(Customer customer)
    {
        Customer = customer;
        CreatedOn = DateTime.Now;
        Items = new List<OrderItem>();
    }

    public int Id { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public Customer Customer { get; private set; }

    public virtual IList<OrderItem> Items { get; private set; }

    public void AddItem(Product product)
    {
        Items.Add(new OrderItem(this, product));
    }
}
