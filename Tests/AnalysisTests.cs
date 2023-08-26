using Microsoft.EntityFrameworkCore;
using System.Text;
using Mapping1;
using Model1;

namespace Tests;

public class AnalysisTests
{
    ApplicationContext _context;
    int _orderId;

    [SetUp]
    public void Setup()
    {
        _context = new ApplicationContext();
        _context.Database.ExecuteSqlRaw("DELETE FROM OrderItems");
        _context.Database.ExecuteSqlRaw("DELETE FROM Orders");
        _context.Database.ExecuteSqlRaw("DELETE FROM Customers");
        _context.Database.ExecuteSqlRaw("DELETE FROM Products");

        var vsc = new Product("Visual Studio Code");
        var vim = new Product("Vim");
        _context.Products.Add(vsc);
        _context.Products.Add(vim);

        var c = new Customer("Sabados Tech", "Argentina");
        _context.Customers.Add(c);

        var o = new Order(c);
        o.AddItem(vsc);
        o.AddItem(vim);
        _context.Orders.Add(o);

        _context.SaveChanges();

        _orderId = o.Id;
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void QueryOrderFullGraph()
    {
        using var readContext = new ApplicationContext();

        var order = readContext.Orders
            .Single(o => o.Id == _orderId);

        var orderText = OrderToString(order);

        TestContext.Out.WriteLine("= ORDER ======================================================");
        TestContext.Out.WriteLine(orderText);
    }

    [Test]
    public void DebugView()
    {
        TestContext.Out.WriteLine("= MODEL ======================================================");
        TestContext.Out.WriteLine(_context.Model.ToDebugString());
        TestContext.Out.WriteLine("= DEBUG ======================================================");
        TestContext.Out.WriteLine(_context.ChangeTracker.DebugView.LongView);
    }

    private static string OrderToString(Order order)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"Order: #{order.Id}, CreatedOn {order.CreatedOn}");
        builder.AppendLine($"  for Customer: {order.Customer.Name} from {order.Customer.Country}");
        foreach ( var item in order.Items )
            builder.AppendLine($"    -> Item: {item.Product.Name} / {item.Quantity}");

        return builder.ToString();
    }
}