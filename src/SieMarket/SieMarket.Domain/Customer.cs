namespace SieMarket.Domain;

public class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public List<Order> Orders { get; set; } = new();

    public decimal GetTotalSpent()
    {
        return Orders.Sum(order => order.GetFinalPrice());
    }
}

