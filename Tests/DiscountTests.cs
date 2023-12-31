using Model1;

namespace Tests;

public class DiscountTests
{
    Customer aCustomer;
    Country argentina, uruguay;
    Category programmingCategory, sportsCategory;
    Product programmingProduct, sportsProduct;

    [SetUp]
    public void Setup()
    {
        argentina = new Country("Argentina");
        uruguay = new Country("Uruguay");
        programmingCategory = new Category("Programming");
        sportsCategory = new Category("Electronics");
        aCustomer = new Customer("Sabados Tech", argentina);
        programmingProduct = new Product("Vim", programmingCategory, 9.95m);
        sportsProduct = new Product("Googles", sportsCategory, 299.95m);
    }

    [Test]
    public void NoDiscountDoesNotChangeTotal()
    {
        var order = new Order(aCustomer);
        order.AddItem(programmingProduct, 1);
        Assert.That(order.Total, Is.EqualTo(9.95m));

        var discount = new Discount(name: "No discount", percentage: 0m);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void CouponTotalDiscountChangeTotal()
    {
        var order = new Order(aCustomer);
        order.AddItem(programmingProduct, 1);
        Assert.That(order.Total, Is.EqualTo(9.95m));

        var discount = new Discount(name: "Coupon", percentage: 10);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m * 0.9m));
    }

    [Test]
    public void SportsCouponDoesNotChangeTotalOnElectronicsOrder()
    {
        var order = new Order(aCustomer);
        order.AddItem(programmingProduct, 1);
        Assert.That(order.Total, Is.EqualTo(9.95m));

        var discount = new DiscountForCategory(name: "Sports coupon", percentage: 8, category: sportsCategory);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void SportsCouponChangeTotalOnSportsOrder()
    {
        var order = new Order(aCustomer);
        order.AddItem(sportsProduct, 1);
        Assert.That(order.Total, Is.EqualTo(299.95m));

        var discount = new DiscountForCategory(name: "Sports coupon", percentage: 8, category: sportsCategory);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(299.95m * 0.92m));
    }

    [Test]
    public void UruguayanCouponDoesNotChangeTotalOnArgentinianOrder()
    {
        var order = new Order(aCustomer);
        order.AddItem(programmingProduct, 1);
        Assert.That(order.Total, Is.EqualTo(9.95m));

        var discount = new DiscountForCountry(name: "Charruan coupon", percentage: 5, country: uruguay);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void SportsCouponAppliesOnlyOnSportsProducts()
    {
        var order = new Order(aCustomer);
        order.AddItem(sportsProduct, 1);        // 299.95m
        order.AddItem(programmingProduct, 1);   //   9.95m
        Assert.That(order.Total, Is.EqualTo(299.95m + 9.95m));

        var discount = new DiscountForCategory(name: "Sports coupon", percentage: 10, category: sportsCategory);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(299.95m * 0.90m + 9.95m));
    }

    [Test]
    public void DiscountOutOfRangeDoesNotDiscounts()
    {
        var order = new Order(aCustomer);
        order.AddItem(programmingProduct, 1);   //   9.95m

        var datedRange = new DateRange(DateTime.MinValue, DateTime.MinValue);
        var discount = new Discount(name: "Oudated coupon", percentage: 10, validOn: datedRange);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void OutOfRangeSportsCouponDoesNotDiscounts()
    {
        var order = new Order(aCustomer);
        order.AddItem(sportsProduct, 1);        // 299.95m

        var datedRange = new DateRange(DateTime.MinValue, DateTime.MinValue);
        var discount = new DiscountForCategory(name: "Sports coupon", percentage: 10, category: sportsCategory, validOn: datedRange);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(299.95m));
    }

    [Test]
    public void OutOfRangeCountryCouponDoesNotDiscounts()
    {
        var order = new Order(aCustomer);
        order.AddItem(programmingProduct, 1);   //   9.95m

        var datedRange = new DateRange(DateTime.MinValue, DateTime.MinValue);
        var discount = new DiscountForCountry(name: "Charruan coupon", percentage: 5, country: argentina, validOn: datedRange);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m));
    }

    [Test]
    public void DiscountInRangeDoesDiscounts()
    {
        var order = new Order(aCustomer);
        order.AddItem(programmingProduct, 1);   //   9.95m

        var currentDateRange = new DateRange(DateTime.MinValue, DateTime.MaxValue);
        var discount = new Discount(name: "Valid coupon", percentage: 10, validOn: currentDateRange);
        order.ApplyDiscount(discount);

        Assert.That(order.Total, Is.EqualTo(9.95m * 0.9m));
    }
}