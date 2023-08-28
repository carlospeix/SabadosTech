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
        if (Country == null)
        {
            return true;
        }

        if (order.Customer.Country.Equals(Country))
        {
            return true;
        }

        return false;
    }
}
