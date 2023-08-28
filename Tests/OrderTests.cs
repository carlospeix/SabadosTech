using Model1;

namespace Tests;

public class OrderTests
{
    Customer aCustomer;
    Country aCountry;
    Category aCategory;
    Product aProduct;

    [SetUp]
    public void Setup()
    {
        aCountry = new Country("Argentina");
        aCategory = new Category("Programming");
        aCustomer = new Customer("Sabados Tech", aCountry);
        aProduct = new Product("Vim", aCategory, 9.95m);
    }

    [Test]
    public void TotalIsCeroForNewOrder()
    {
        var order = new Order(aCustomer);

        Assert.That(order.Total, Is.EqualTo(0));
    }

    [Test]
    public void TotalOrderForOneItemOrder()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void TotalOrderForTwoItemsOrder()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 2);

        Assert.That(order.Total, Is.EqualTo(9.95m * 2));
    }

    [Test]
    public void TotalOrderPriceDoNotChangeIfProductPriceChange()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);
        aProduct.ChangePrice(5.0m);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void TotalOrderPriceChangeIfQuantityIncreases()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);
        order.IncreaseQuantityFor(aProduct);

        Assert.That(order.Total, Is.EqualTo(9.95m * 2));
    }

    [Test]
    public void TotalOrderPriceChangeIfQuantityDecreases()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 2);
        order.DecreaseQuantityFor(aProduct);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void RemovesItemIfQuantityIsZero()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);
        order.DecreaseQuantityFor(aProduct);

        Assert.That(order.Items, Is.Empty);
    }

    [Test]
    public void DoesNotRemoveItemForNotIncludedProduct()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);
        order.DecreaseQuantityFor(new Product("Notepad", aCategory, 10.0m));

        Assert.That(order.Items, Has.Count.EqualTo(1));
    }

    [Test]
    public void CanRemoveItem()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);
        order.RemoveItemFor(aProduct);

        Assert.That(order.Items, Is.Empty);
        Assert.That(order.Total, Is.EqualTo(0));
    }
}