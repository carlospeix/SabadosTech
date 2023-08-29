using Mapping1;
using Microsoft.EntityFrameworkCore;
using Model1;

namespace Tests;

public class PersistenceTests
{
    ApplicationContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new ApplicationContext();
        _context.Orders.RemoveRange(_context.Orders);
        _context.SaveChanges();

        var vsc = _context.Products.Single(c => c.Id == 1);
        var vim = _context.Products.Single(c => c.Id == 2);
        var c = _context.Customers.Single(c => c.Id == 1);

        _context.Orders.Add(CreateOrder());

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.ChangeTracker.Clear();
        _context.Orders.RemoveRange(_context.Orders);
        _context.Discounts.RemoveRange(_context.Discounts);
        _context.SaveChanges();
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
            // ... because OrderItems is not exposed
            Assert.That(_context.Set<OrderItem>().Count, Is.EqualTo(2));
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
    public void ShouldFailBecauseOfCateogryReferentialIntegrity()
    {
        var prog = _context.Categories.Single(c => c.Id == 2);
        Assert.Throws<InvalidOperationException>(delegate {
            _context.Categories.Remove(prog);
            _context.SaveChanges();
        });

        Assert.Multiple(() =>
        {
            Assert.That(_context.Products.Count(), Is.EqualTo(6));
            Assert.That(_context.Categories.Count(), Is.EqualTo(4));
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
            // ... because OrderItems is not exposed
            Assert.That(_context.Set<OrderItem>().Count, Is.EqualTo(0));
        });
    }

    [Test]
    public void SavingDiscountsHierarchy()
    {
        var discount = new Discount("General discount", 10);
        var categoryDiscount = new DiscountForCategory("Category discount", 10, _context.Categories.First());
        var countryDiscount = new DiscountForCountry("Country discount", 10, _context.Countries.First());
        _context.Discounts.AddRange(discount, categoryDiscount, countryDiscount);
        _context.SaveChanges();

        Assert.That(_context.Discounts.Count, Is.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(_context.Discounts.Single(d => d.Id == discount.Id), Is.TypeOf(typeof(Discount)));
            Assert.That(_context.Discounts.Single(d => d.Id == categoryDiscount.Id), Is.TypeOf(typeof(DiscountForCategory)));
            Assert.That(_context.Discounts.Single(d => d.Id == countryDiscount.Id), Is.TypeOf(typeof(DiscountForCountry)));
        });
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
