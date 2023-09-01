using System.Text.Json;
using System.Text;
using Application;
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
        var ordersApp = new Orders();
        var order = ordersApp.CreateOrder(customerId: 1)!;

        ordersApp.AddProductToOrder(orderId: order.Id, productId: 1, quantity: 1);
        ordersApp.AddProductToOrder(orderId: order.Id, productId: 2, quantity: 1);

        try
        {
            var orderText = OrderToString(ordersApp.GetById(order.Id));

            TestContext.Out.WriteLine("= ORDER ======================================================");
            TestContext.Out.WriteLine(orderText);
        }
        finally
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }

    [Test, Explicit]
    public void GetDiscountsLike()
    {
        var ordersApp = new Orders();
        var discounts = ordersApp.GetDiscountsByName("");

        TestContext.Out.WriteLine(SerializeResult(discounts));
    }

    [Test, Explicit]
    public void ReportSalesByCountryAndCategory()
    {
        var reportsApp = new Reports();

        var report = reportsApp.SalesByCountryAndCategory();
        //var reportCategory = reportsApp.SalesByCountryAndCategory(categoryId: 2);
        //var reportCountry = reportsApp.SalesByCountryAndCategory(countryId: 1);
        //var reportCountryAndCategory = reportsApp.SalesByCountryAndCategory(countryId: 1, categoryId: 2);
        //var reportCountriesAndCategories = reportsApp.SalesByCountriesAndCategories(countryIds: new int[] { 1, 2 }, categoryIds: new int[] { 3, 4 });
        //var reportCountriesAndCategories = reportsApp.SalesByCountriesAndCategories(countryIds: new int[] { 1 }, categoryIds: new int[] { 2 });

        TestContext.Out.WriteLine(SerializeResult(report));
        //TestContext.Out.WriteLine(SerializeResult(reportCategory));
        //TestContext.Out.WriteLine(SerializeResult(reportCountry));
        //TestContext.Out.WriteLine(SerializeResult(reportCountryAndCategory));
        //TestContext.Out.WriteLine(SerializeResult(reportCountriesAndCategories));
    }

    [Test, Explicit]
    public void ApplyDiscountToOrder()
    {
        var orderId = _context.Orders.First().Id;
        var discountName = "BLACK-FRIDAY-SPORTS";

        var ordersApp = new Orders();
        _ = ordersApp.ApplyDiscount(orderId: orderId, discountName: discountName);
    }

    [Test, Explicit]
    public void XCreateOrderAndDiscount()
    {
        var sports = _context.Categories.Single(c => c.Id == 3);  // Sports
        var validityRange = new DateRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10));
        var discount = new DiscountForCategory(name: "BLACK-FRIDAY-SPORTS", percentage: 10, sports, validityRange);
        _context.Discounts.Add(discount);

        var customer = _context.Customers.Single(c => c.Id == 1);
        var order = new Order(customer);
        order.AddItem(_context.Products.Single(c => c.Id == 3), 1); // Surf appliance
        order.AddItem(_context.Products.Single(c => c.Id == 2), 1); // Vim
        _context.Orders.Add(order);

        _context.SaveChanges();
    }

    [Test, Explicit]
    public void XRemoveOrderAndDiscount()
    {
        _context.Orders.RemoveRange(_context.Orders);
        _context.Discounts.RemoveRange(_context.Discounts);
        _context.SaveChanges();
    }

    [Test, Explicit]
    public void XGenerate20Orders()
    {
        CreateOrderBulk();
    }

    [Test, Explicit]
    public void XRemoveOrders()
    {
        _context.Orders.RemoveRange(_context.Orders);
        _context.SaveChanges();
    }

    [Test]
    public void DebugView()
    {
        TestContext.Out.WriteLine("= MODEL ======================================================");
        TestContext.Out.WriteLine(_context.Model.ToDebugString());
        TestContext.Out.WriteLine("= DEBUG ======================================================");
        TestContext.Out.WriteLine(_context.ChangeTracker.DebugView.LongView);
    }

    private static string OrderToString(Order? order)
    {
        if (order == null)
            return "order is null";

        var builder = new StringBuilder();

        builder.AppendLine($"Order: #{order.Id}, CreatedOn {order.CreatedOn}");
        builder.AppendLine($"  for Customer: {order.Customer.Name} from {order.Customer.Country}");
        foreach ( var item in order.Items )
            builder.AppendLine($"    -> Item: {item.Product.Name} / {item.Quantity}");

        return builder.ToString();
    }

    private void CreateOrderBulk()
    {
        var rnd = new Random();

        for (int i = 0; i < 20; i++)
        {
            var customerId = rnd.Next(1, 5);
            var productId = rnd.Next(1, 7);
            var quantity = rnd.Next(1, 15);
            var order = new Order(_context.Customers.Single(c => c.Id == customerId));
            order.AddItem(_context.Products.Single(c => c.Id == productId), quantity);
            _context.Orders.Add(order);
        }

        _context.SaveChanges();
    }

    private string SerializeResult(object? result)
    {
        JsonSerializerOptions options = new(JsonSerializerDefaults.Web) { WriteIndented = true };
        return System.Text.Json.JsonSerializer.Serialize(result, options);
    }
}