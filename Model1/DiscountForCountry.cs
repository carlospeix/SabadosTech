namespace Model1;

public class DiscountForCountry : Discount
{
    public DiscountForCountry(string name, decimal percentage = 0m, Country? country = default) : base(name, percentage)
    {
        Country = country;
    }

    public Country? Country { get; private set; }

    public override bool AppliesTo(Order order)
    {
        return Country == null || order.Customer.Country.Equals(Country);
    }

    public override decimal Apply(Order order)
    {
        return order.Customer.Country != Country ? 0 : order.ItemsTotal * Percentage / 100;
    }
}
