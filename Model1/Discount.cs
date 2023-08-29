namespace Model1;

public class Discount
{
    protected Discount() { }

    public Discount(string name, decimal percentage, DateRange? validOn = null)
    {
        Name = name;
        Percentage = percentage;
        ValidOn = validOn ?? DateRange.Infinity;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public decimal Percentage { get; private set; }
    public DateRange ValidOn { get; private set; }

    public virtual bool AppliesTo(Order order)
    {
        return ValidOn.Includes(order.CreatedOn);
    }

    public virtual decimal Apply(Order order)
    {
        return order.ItemsTotal * Percentage / 100;
    }
}
