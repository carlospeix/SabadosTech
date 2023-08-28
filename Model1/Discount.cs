﻿namespace Model1;

public class Discount
{
    public Discount(string name, decimal percentage = 0m)
    {
        Name = name;
        Percentage = percentage;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public decimal Percentage { get; set; }

    internal bool AppliesTo(Order order)
    {
        return true;
    }
}