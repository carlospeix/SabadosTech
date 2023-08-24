namespace Model1;

public class OrderItem
{
    public int Id { get; private set; }

    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
    public virtual int Quantity { get; set; }
}
