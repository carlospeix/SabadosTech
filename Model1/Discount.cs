namespace Model1;

public class Discount
{
    public Discount(string name, decimal percentage = 0m, Category? category = default)
    {
        Name = name;
        Percentage = percentage;
        Category = category;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public decimal Percentage { get; private set; }
    public Category? Category { get; private set; }

    internal bool AppliesTo(Order order)
    {
        if (Category == null)
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

        return false;
    }
}
