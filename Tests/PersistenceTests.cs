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

        var vsc = _context.Products.Single(c => c.Id == 1);
        var vim = _context.Products.Single(c => c.Id == 2);
        var arg = _context.Countries.Single(c => c.Id == 1);
        var c = _context.Customers.Single(c => c.Id == 1);

        var o = new Order(c);
        o.AddItem(vsc);
        o.AddItem(vim);
        _context.Orders.Add(o);

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM OrderItems");
        _context.Database.ExecuteSqlRaw("DELETE FROM Orders");
        _context.Dispose();
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
    public void ShouldFailBecauseOfProductReferentialIntegrity()
    {
        var vsc = _context.Products.Single(c => c.Id == 1);
        Assert.Throws<InvalidOperationException>(delegate {
            _context.Products.Remove(vsc);
            _context.SaveChanges();
        });

        Assert.Multiple(() =>
        {
            Assert.That(_context.Database.SqlQuery<int>($"SELECT COUNT(*) FROM OrderItems").ToArray()[0], Is.EqualTo(2));
            Assert.That(_context.Products.Count(), Is.EqualTo(6));
        });
    }

    [Test]
    public void ShouldFailBecauseOfCustomerReferentialIntegrity()
    {
        var c = _context.Customers.Single(c => c.Id == 1);
        Assert.Throws<InvalidOperationException>(delegate {
            _context.Customers.Remove(c);
            _context.SaveChanges();
        });

        Assert.Multiple(() =>
        {
            Assert.That(_context.Orders.Count(), Is.EqualTo(1));
            Assert.That(_context.Customers.Count(), Is.EqualTo(4));
        });
    }

    [Test]
    public void ShouldFailBecauseOfCountryReferentialIntegrity()
    {
        var arg = _context.Countries.Single(c => c.Id == 1);
        Assert.Throws<InvalidOperationException>(delegate {
            _context.Countries.Remove(arg);
            _context.SaveChanges();
        });

        Assert.Multiple(() =>
        {
            Assert.That(_context.Orders.Count(), Is.EqualTo(1));
            Assert.That(_context.Countries.Count(), Is.EqualTo(6));
        });
    }

    [Test]
    public void CascadingDeleteOnOrder()
    {
        var o = _context.Orders.First();
        _context.Orders.Remove(o);
        _context.SaveChanges();

        Assert.Multiple(() =>
        {
            Assert.That(_context.Products.Count, Is.EqualTo(6));
            Assert.That(_context.Customers.Count, Is.EqualTo(4));
            Assert.That(_context.Orders.Count, Is.EqualTo(0));
            // Hack, because OrderItems is not exposed
            Assert.That(_context.Database.SqlQuery<int>($"SELECT COUNT(*) FROM OrderItems").ToArray()[0], Is.EqualTo(0));
        });
    }
}
