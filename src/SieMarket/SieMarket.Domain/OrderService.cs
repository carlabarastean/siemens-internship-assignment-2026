namespace SieMarket.Domain;

public class OrderService
{
    public static string? FindTopSpendingCustomer(IEnumerable<Customer> customers)
    {
        if (customers == null || !customers.Any())
        {
            return null;
        }

        var topCustomer = customers
            .OrderByDescending(customer => customer.GetTotalSpent())
            .FirstOrDefault();

        return topCustomer?.Name;
    }

    public static Customer? FindTopSpendingCustomerObject(IEnumerable<Customer> customers)
    {
        if (customers == null || !customers.Any())
        {
            return null;
        }

        return customers
            .OrderByDescending(customer => customer.GetTotalSpent())
            .FirstOrDefault();
    }
}


