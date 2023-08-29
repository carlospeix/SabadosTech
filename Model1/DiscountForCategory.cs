namespace Model1;

public class DiscountForCategory : Discount
{
    protected DiscountForCategory() : base() { }

    public DiscountForCategory(string name, decimal percentage, Category category, DateRange? validOn = null) :
        base(name, percentage, validOn)
    {
        Category = category;
    }

    public Category Category { get; private set; }

    public override bool AppliesTo(Order order)
    {
        return base.AppliesTo(order) &&
            order.Items.Any(item => item.Product.Category.Equals(Category));
    }

    public override decimal Apply(Order order)
    {
        return order.Items
            .Where(item => item.Product.Category == Category)
            .Sum(item => item.Total * Percentage / 100);
    }
}
