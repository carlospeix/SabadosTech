namespace Model1;

public class Discount
{
    public Discount(string name, decimal percentage)
    {
        Name = name;
        Percentage = percentage;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public decimal Percentage { get; private set; }

    public virtual bool AppliesTo(Order order)
    {
        return true;
    }

    public virtual decimal Apply(Order order)
    {
        return order.ItemsTotal * Percentage / 100;
    }
}
