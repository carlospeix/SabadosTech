namespace Model1;

public class Order
{
    private Order() { }

    public Order(Customer customer)
    {
        Customer = customer;
        CreatedOn = DateTime.Now;
    }

    public int Id { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public Customer Customer { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.ToList().AsReadOnly();
    private readonly HashSet<OrderItem> _items = new();

    public void AddItem(Product product, int quantity = 1)
    {
        _items.Add(new OrderItem(this, product, quantity));
    }
}
