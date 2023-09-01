namespace Application;

public class Reports
{
    public object SalesByCountryAndCategory(int? countryId = null, int? categoryId = null)
    {
        using var context = new ApplicationContext();

        return context.Set<OrderItem>()
            .Where(oi => !countryId.HasValue || oi.Order.Customer.Country.Id == countryId)
            .Where(oi => !categoryId.HasValue || oi.Product.Category.Id == categoryId)
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

    public object SalesByCountriesAndCategories(int[] countryIds, int[] categoryIds)
    {
        using var context = new ApplicationContext();

        return context.Set<OrderItem>()
            .Where(oi => countryIds.Contains(oi.Order.Customer.Country.Id))
            .Where(oi => categoryIds.Contains(oi.Product.Category.Id))
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