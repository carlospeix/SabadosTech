namespace Model1;

public class DiscountForCountry : Discount
{
    protected DiscountForCountry() : base() { }

    public DiscountForCountry(string name, decimal percentage, Country country, DateRange? validOn = null) :
        base(name, percentage, validOn)
    {
        Country = country;
    }

    public Country Country { get; private set; }

    public override bool AppliesTo(Order order)
    {
        return base.AppliesTo(order) && order.Customer.Country.Equals(Country);
    }

    public override decimal Apply(Order order)
    {
        return order.Customer.Country != Country ? 0 : order.ItemsTotal * Percentage / 100;
    }
}
