namespace Model1;

public class DiscountForCategory : Discount
{
    public DiscountForCategory(string name, decimal percentage = 0m, Category? category = default) : base(name, percentage)
    {
        Category = category;
    }

    public Category? Category { get; private set; }

    public override bool AppliesTo(Order order)
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

    public override decimal Apply(Order order)
    {
        decimal discount = 0;

        foreach (var orderItem in order.Items)
        {
            if (orderItem.Product.Category.Equals(Category))
            {
                discount += orderItem.Total * Percentage / 100;
            }
        }

        return discount;
    }
}
