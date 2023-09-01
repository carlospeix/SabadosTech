using Model1;

namespace Application;

public class Orders
{
    public Order? GetById(int orderId)
    {
        using var context = new ApplicationContext();

        return context.Orders.FirstOrDefault(o => o.Id == orderId);
    }

    public Order? CreateOrder(int customerId)
    {
        using var context = new ApplicationContext();

        var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            return null;
        }

        var order = new Order(customer);

        context.Orders.Add(order);
        context.SaveChanges();

        return order;
    }

    public Order? AddProductToOrder(int orderId, int productId, int quantity)
    {
        using var context = new ApplicationContext();

        var order = context.Orders.FirstOrDefault(c => c.Id == orderId);
        if (order == null)
        {
            return null;
        }

        var product = context.Products.FirstOrDefault(c => c.Id == productId);
        if (product == null)
        {
            return null;
        }

        if (quantity <= 0)
        {
            return null;
        }

        order.AddItem(product, quantity);

        context.SaveChanges();

        return order;
    }

    public IReadOnlyList<Discount> GetDiscountsByName(string search)
    {
        using var context = new ApplicationContext();

        return context.Discounts
            .Where(o => o.Name.Contains(search))
            .ToList();
    }

    public Order? ApplyDiscount(int orderId, string discountName)
    {
        using var context = new ApplicationContext();

        var order = context.Orders.FirstOrDefault(c => c.Id == orderId);
        if (order == null)
        {
            return null;
        }

        var discount = context.Discounts.FirstOrDefault(o => o.Name == discountName);
        if (discount == null)
        {
            return null;
        }

        order.ApplyDiscount(discount);
        
        context.SaveChanges();

        return order;
    }
}
