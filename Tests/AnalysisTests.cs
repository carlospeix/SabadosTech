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

        var vsc = _context.Products.Single(c => c.Id == 1);
        var vim = _context.Products.Single(c => c.Id == 2);
        var arg = _context.Countries.Single(c => c.Id == 1);
        var c = _context.Customers.Single(c => c.Id == 1);

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
        _context.Database.ExecuteSqlRaw("DELETE FROM OrderItems");
        _context.Database.ExecuteSqlRaw("DELETE FROM Orders");
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