using System.Text;
using Mapping1;
using Model1;

namespace Tests;

public class AnalysisTests
{
    ApplicationContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new ApplicationContext();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void QueryOrderFullGraph()
    {
        var order = CreateOrder();
        _context.Orders.Add(order);
        _context.SaveChanges();

        try
        {
            using var readContext = new ApplicationContext();

            var orderText = OrderToString(
                readContext.Orders.Single(o => o.Id == order.Id));

            TestContext.Out.WriteLine("= ORDER ======================================================");
            TestContext.Out.WriteLine(orderText);
        }
        finally
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
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

    private Order CreateOrder()
    {
        var vsc = _context.Products.Single(c => c.Id == 1);
        var vim = _context.Products.Single(c => c.Id == 2);
        var customer = _context.Customers.Single(c => c.Id == 1);

        var order = new Order(customer);
        order.AddItem(vsc);
        order.AddItem(vim);

        return order;
    }

}