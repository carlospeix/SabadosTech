namespace Model1;

public class Discount
{
    public Discount(string name, decimal percentage = 0m, Category? category = default, Country? country = default)
    {
        Name = name;
        Percentage = percentage;
        Category = category;
        Country = country;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public decimal Percentage { get; private set; }
    public Category? Category { get; private set; }
    public Country? Country { get; }

    internal bool AppliesTo(Order order)
    {
        if (Category == null && Country == null)
        {
            return true;
        }

        foreach (var orderItem in order.Items)
        {
            if (orderItem.Product.Category.Equals(Category))
            {
                return true;
            }
        }

        if (order.Customer.Country.Equals(Country))
        {
            return true;
        }

        return false;
    }
}
