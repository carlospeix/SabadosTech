namespace Model1;

public class Product
{
    private Product() { }

    public Product(string name, Category category)
    {
        Name = name;
        Category = category;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public Category Category { get; set; }
}
