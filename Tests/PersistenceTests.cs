using Microsoft.EntityFrameworkCore;
using Mapping1;
using Model1;

namespace Tests;

public class PersistenceTests
{
    ApplicationContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new ApplicationContext();
        _context.Database.ExecuteSqlRaw("DELETE FROM OrderItems");
        _context.Database.ExecuteSqlRaw("DELETE FROM Orders");
        _context.Database.ExecuteSqlRaw("DELETE FROM Customers");
        _context.Database.ExecuteSqlRaw("DELETE FROM Products");

        var vsc = new Product() { Name = "Visual Studio Code" };
        var vim = new Product() { Name = "Vim" };
        _context.Products.Add(vsc);
        _context.Products.Add(vim);

        var c = new Customer() { Name = "Sabados Tech", Country = "Argentina" };
        _context.Customers.Add(c);

        var o = new Order() { Customer = c, CreatedOn = DateTime.Now };
        o.Items.Add(new OrderItem() { Product = vsc });
        o.Items.Add(new OrderItem() { Product = vim });
        _context.Orders.Add(o);

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void CanCreateTwoProducts()
    {
        Assert.That(_context.Products.Count(), Is.EqualTo(2));
    }

    [Test]
    public void CanCreateACustomer()
    {
        Assert.That(_context.Customers.Count(), Is.EqualTo(1));
    }

    [Test]
    public void CanCreateAnOrder()
    {
        Assert.That(_context.Orders.Count(), Is.EqualTo(1));
    }

    [Test]
    public void CanCreateAnOrderWithItems()
    {
        Assert.That(_context.Orders.First().Items, Has.Count.EqualTo(2));
    }

    [Test]
    public void CascadingDeleteOnOrder()
    {
        var o = _context.Orders.First();
        _context.Orders.Remove(o);
        _context.SaveChanges();

        Assert.Multiple(() =>
        {
            Assert.That(_context.Products.Count, Is.EqualTo(2));
            Assert.That(_context.Customers.Count, Is.EqualTo(1));
            Assert.That(_context.Orders.Count, Is.EqualTo(0));
            // Hack, because OrderItems is not exposed
            Assert.That(_context.Database.SqlQuery<int>($"SELECT COUNT(*) FROM OrderItems").ToArray()[0], Is.EqualTo(0));
        });
    }
}