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

    public decimal Total => _items.Sum(oi => oi.Total);

    public void IncreaseQuantityFor(Product aProduct)
    {
        var item = _items.FirstOrDefault(oi => oi.Product == aProduct);
        if (item == null)
        {
            return;
        }
        item.IncreaseQuantity();
    }

    public void DecreaseQuantityFor(Product aProduct)
    {
        var item = _items.FirstOrDefault(oi => oi.Product == aProduct);
        if (item == null)
        {
            return;
        }
        if (item.Quantity == 1)
        {
            _items.Remove(item);
        }
        else
        {
            item.DecreaseQuantity();
        }   
    }
}
