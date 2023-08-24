namespace Model1;

public class Product
{
    private Product() { }

    public Product(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
}
