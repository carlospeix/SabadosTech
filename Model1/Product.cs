namespace Model1;

public class Product
{
    private Product() { }

    public Product(string name, Category category, decimal price = 10) // TODO: Remove default
    {
        Name = name;
        Category = category;
        Price = price;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; private set; }

    public void ChangePrice(decimal newPrice)
    {
        Price = newPrice;
    }
}
