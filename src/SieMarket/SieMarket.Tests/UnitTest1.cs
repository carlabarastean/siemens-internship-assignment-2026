using SieMarket.Domain;

namespace SieMarket.Tests;

public class OrderTests
{
    [Fact]
    public void OrderItem_CalculatesTotalPrice_Correctly()
    {
        var orderItem = new OrderItem
        {
            ProductName = "Laptop",
            Quantity = 2,
            UnitPrice = 500m
        };

        var totalPrice = orderItem.GetTotalPrice();

        Assert.Equal(1000m, totalPrice);
    }

    [Fact]
    public void Order_GetSubtotal_CalculatesCorrectly()
    {
        var order = new Order
        {
            OrderId = 1,
            OrderDate = DateTime.Now,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 800m },
                new() { ProductName = "Mouse", Quantity = 2, UnitPrice = 25m }
            }
        };

        var subtotal = order.GetSubtotal();

        Assert.Equal(850m, subtotal);
    }

    [Fact]
    public void Order_GetFinalPrice_NoDiscount_WhenBelowThreshold()
    {
        var order = new Order
        {
            OrderId = 1,
            OrderDate = DateTime.Now,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Keyboard", Quantity = 2, UnitPrice = 100m },
                new() { ProductName = "Mouse", Quantity = 1, UnitPrice = 50m }
            }
        };

        var finalPrice = order.GetFinalPrice();
        var discount = order.GetDiscountAmount();

        Assert.Equal(250m, finalPrice);
        Assert.Equal(0m, discount);
    }

    [Fact]
    public void Order_GetFinalPrice_Applies10PercentDiscount_WhenAbove500()
    {
        var order = new Order
        {
            OrderId = 1,
            OrderDate = DateTime.Now,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 600m }
            }
        };

        var finalPrice = order.GetFinalPrice();
        var discount = order.GetDiscountAmount();
        var subtotal = order.GetSubtotal();

        Assert.Equal(600m, subtotal);
        Assert.Equal(60m, discount); // 10% of 600
        Assert.Equal(540m, finalPrice); // 600 - 60
    }

    [Fact]
    public void Order_GetFinalPrice_NoDiscount_WhenExactly500()
    {
        var order = new Order
        {
            OrderId = 1,
            OrderDate = DateTime.Now,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Monitor", Quantity = 1, UnitPrice = 500m }
            }
        };

        var finalPrice = order.GetFinalPrice();
        var discount = order.GetDiscountAmount();

        Assert.Equal(500m, finalPrice);
        Assert.Equal(0m, discount); // No discount at exactly 500
    }

    [Fact]
    public void Customer_GetTotalSpent_CalculatesAcrossMultipleOrders()
    {
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Klaus Schmidt",
            Email = "klaus.schmidt@t-online.de",
            Country = "Germany"
        };

        var order1 = new Order
        {
            OrderId = 1,
            Customer = customer,
            OrderDate = DateTime.Now.AddDays(-10),
            Items = new List<OrderItem>
            {
                new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 600m }
            }
        };

        var order2 = new Order
        {
            OrderId = 2,
            Customer = customer,
            OrderDate = DateTime.Now,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Mouse", Quantity = 2, UnitPrice = 50m }
            }
        };

        customer.Orders = new List<Order> { order1, order2 };

        var totalSpent = customer.GetTotalSpent();

        // Order 1: 600 - 60 (10% discount) = 540
        // Order 2: 100 (no discount) = 100
        // Total: 640
        Assert.Equal(640m, totalSpent);
    }

    [Fact]
    public void OrderService_FindTopSpendingCustomer_ReturnsCustomerWithHighestSpending()
    {
        var customer1 = new Customer
        {
            CustomerId = 1,
            Name = "Sophie Lefevre",
            Email = "sophie.lefevre@orange.fr",
            Country = "France"
        };
        customer1.Orders = new List<Order>
        {
            new()
            {
                OrderId = 1,
                Customer = customer1,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 800m }
                }
            }
        };

        var customer2 = new Customer
        {
            CustomerId = 2,
            Name = "Miguel Rodríguez",
            Email = "miguel.rodriguez@telefonica.es",
            Country = "Spain"
        };
        customer2.Orders = new List<Order>
        {
            new()
            {
                OrderId = 2,
                Customer = customer2,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new() { ProductName = "Tablet", Quantity = 1, UnitPrice = 1200m }
                }
            }
        };

        var customer3 = new Customer
        {
            CustomerId = 3,
            Name = "Marco Bianchi",
            Email = "marco.bianchi@alice.it",
            Country = "Italy"
        };
        customer3.Orders = new List<Order>
        {
            new()
            {
                OrderId = 3,
                Customer = customer3,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new() { ProductName = "Phone", Quantity = 1, UnitPrice = 400m }
                }
            }
        };

        var customers = new List<Customer> { customer1, customer2, customer3 };

        var topCustomer = OrderService.FindTopSpendingCustomer(customers);

        Assert.Equal("Miguel Rodríguez", topCustomer);
    }

    [Fact]
    public void OrderService_FindTopSpendingCustomer_ReturnsNull_WhenNoCustomers()
    {
        var customers = new List<Customer>();

        var topCustomer = OrderService.FindTopSpendingCustomer(customers);

        Assert.Null(topCustomer);
    }

    [Fact]
    public void OrderService_FindTopSpendingCustomerObject_ReturnsCustomerObject()
    {
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test Customer",
            Email = "test@example.com",
            Country = "Germany"
        };
        customer.Orders = new List<Order>
        {
            new()
            {
                OrderId = 1,
                Customer = customer,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 1000m }
                }
            }
        };

        var customers = new List<Customer> { customer };

        var topCustomer = OrderService.FindTopSpendingCustomerObject(customers);

        Assert.NotNull(topCustomer);
        Assert.Equal("Test Customer", topCustomer.Name);
        Assert.Equal(900m, topCustomer.GetTotalSpent()); // 1000 - 100 (10% discount)
    }

    [Fact]
    public void Order_ComplexScenario_MultipleItemsWithDiscount()
    {
        var order = new Order
        {
            OrderId = 1,
            OrderDate = DateTime.Now,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Gaming Laptop", Quantity = 1, UnitPrice = 1500m },
                new() { ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 40m },
                new() { ProductName = "Mechanical Keyboard", Quantity = 1, UnitPrice = 150m },
                new() { ProductName = "USB Cable", Quantity = 3, UnitPrice = 10m }
            }
        };

        var subtotal = order.GetSubtotal();
        var discount = order.GetDiscountAmount();
        var finalPrice = order.GetFinalPrice();

        // Subtotal: 1500 + 80 + 150 + 30 = 1760
        Assert.Equal(1760m, subtotal);
        // Discount: 10% of 1760 = 176
        Assert.Equal(176m, discount);
        // Final: 1760 - 176 = 1584
        Assert.Equal(1584m, finalPrice);
    }
}

