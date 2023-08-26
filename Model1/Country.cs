namespace Model1;

public class Country
{
    public Country(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
}
