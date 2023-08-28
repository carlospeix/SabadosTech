namespace Application;

public class Reports
{
    public object SalesByCountryAndCategory()
    {
        using var context = new ApplicationContext();

        return context.Set<OrderItem>()
            .GroupBy(oi => new
            {
                CountryName = oi.Order.Customer.Country.Name,
                CategoryName = oi.Product.Category.Name
            })
            .Select(group => new
            {
                group.Key.CountryName,
                group.Key.CategoryName,
                Quantity = group.Sum(oi => oi.Quantity),
                Amount = group.Sum(oi => oi.Total)
            })
            .ToList();
    }
}