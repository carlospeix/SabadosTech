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
        UpdateTotals();
    }

    public void RemoveItemFor(Product aProduct)
    {
        var item = _items.FirstOrDefault(oi => oi.Product == aProduct);
        if (item == null)
        {
            return;
        }
        _items.Remove(item);
        UpdateTotals();
    }

    public decimal ItemsTotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }

    public void IncreaseQuantityFor(Product aProduct)
    {
        var item = _items.FirstOrDefault(oi => oi.Product == aProduct);

        if (item == null)
        {
            return;
        }
        item.IncreaseQuantity();

        UpdateTotals();
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

        UpdateTotals();
    }

    public void ApplyDiscount(Discount discount)
    {
        if (discount.AppliesTo(this))
        {
            if (!_discounts.Contains(discount))
            {
                _discounts.Add(discount);
            }
        }

        UpdateTotals();
    }
    public IReadOnlyCollection<Discount> Discounts => _discounts.ToList().AsReadOnly();
    private readonly HashSet<Discount> _discounts = new();

    private void UpdateTotals()
    {
        ItemsTotal = _items.Sum(oi => oi.Total);

        Discount = _discounts
            .Sum(discount => discount.Apply(this));

        Total = ItemsTotal - Discount;
    }
}
