using Model1;

namespace Tests;

public class DiscountTests
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
    public void NoDiscountDoesNotChangeTotal()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);
        Assert.That(order.Total, Is.EqualTo(9.95m));

        var discount = new Discount("No discount");
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void CouponTotalDiscountChangeTotal()
    {
        var order = new Order(aCustomer);
        order.AddItem(aProduct, 1);
        Assert.That(order.Total, Is.EqualTo(9.95m));

        var discount = new Discount(name: "Coupon", percentage: 10);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m * 0.9m));
    }
}