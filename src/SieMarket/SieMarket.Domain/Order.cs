namespace SieMarket.Domain;

public class Order
{
    private const decimal DiscountThreshold = 500m;
    private const decimal DiscountRate = 0.10m;

    public int OrderId { get; set; }

    public Customer Customer { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public List<OrderItem> Items { get; set; } = new();

    public decimal GetSubtotal()
    {
        return Items.Sum(item => item.GetTotalPrice());
    }

    public decimal GetDiscountAmount()
    {
        var subtotal = GetSubtotal();
        return subtotal > DiscountThreshold ? subtotal * DiscountRate : 0m;
    }

    public decimal GetFinalPrice()
    {
        var subtotal = GetSubtotal();
        var discount = GetDiscountAmount();
        return subtotal - discount;
    }
}

