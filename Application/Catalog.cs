namespace Application;

public class Catalog
{
    public IReadOnlyList<Product> ProductsByFilter(int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null, bool inAscendingOrder = true)
    {
        using var context = new ApplicationContext();

        var query = context.Products
            .Where(p => !categoryId.HasValue || p.Category.Id == categoryId)
            .Where(p => !minPrice.HasValue || p.Price >= minPrice)
            .Where(p => !maxPrice.HasValue || p.Price <= maxPrice);

        if (inAscendingOrder)
        {
            return query.OrderBy(p => p.Price).ToList();
        }
        else
        {
            return query.OrderByDescending(p => p.Price).ToList();
        }
    }
}