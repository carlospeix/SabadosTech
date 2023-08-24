namespace Model1;

public class Customer
{
    private Customer() { }

    public Customer(string name, string country)
    {
        Name = name;
        Country = country;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public string Country { get; set; }
}
