using SieMarket.Domain;

namespace SieMarket.Demo;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SieMarket - Online Electronics Store Demo ===\n");

        var customers = CreateSampleCustomers();

        DisplayAllOrders(customers);

        DisplayTopSpendingCustomer(customers);
    }

    static List<Customer> CreateSampleCustomers()
    {
        var alice = new Customer
        {
            CustomerId = 1,
            Name = "Marie Dubois",
            Email = "marie.dubois@wanadoo.fr",
            Country = "France"
        };

        var aliceOrder = new Order
        {
            OrderId = 1,
            Customer = alice,
            OrderDate = new DateTime(2026, 2, 15),
            Items = new List<OrderItem>
            {
                new() { ProductName = "Gaming Laptop", Quantity = 1, UnitPrice = 1500m },
                new() { ProductName = "Wireless Mouse", Quantity = 1, UnitPrice = 45m },
                new() { ProductName = "USB-C Cable", Quantity = 2, UnitPrice = 15m }
            }
        };

        alice.Orders = new List<Order> { aliceOrder };

        var bob = new Customer
        {
            CustomerId = 2,
            Name = "Carlos García",
            Email = "carlos.garcia@correo.es",
            Country = "Spain"
        };

        var bobOrder1 = new Order
        {
            OrderId = 2,
            Customer = bob,
            OrderDate = new DateTime(2026, 2, 20),
            Items = new List<OrderItem>
            {
                new() { ProductName = "4K Monitor", Quantity = 2, UnitPrice = 350m }
            }
        };

        var bobOrder2 = new Order
        {
            OrderId = 3,
            Customer = bob,
            OrderDate = new DateTime(2026, 2, 25),
            Items = new List<OrderItem>
            {
                new() { ProductName = "Mechanical Keyboard", Quantity = 1, UnitPrice = 120m },
                new() { ProductName = "Webcam HD", Quantity = 1, UnitPrice = 80m }
            }
        };

        bob.Orders = new List<Order> { bobOrder1, bobOrder2 };

        var carol = new Customer
        {
            CustomerId = 3,
            Name = "Giulia Rossi",
            Email = "giulia.rossi@libero.it",
            Country = "Italy"
        };

        var carolOrder = new Order
        {
            OrderId = 4,
            Customer = carol,
            OrderDate = new DateTime(2026, 2, 22),
            Items = new List<OrderItem>
            {
                new() { ProductName = "Tablet Pro", Quantity = 1, UnitPrice = 450m },
                new() { ProductName = "Tablet Case", Quantity = 1, UnitPrice = 35m }
            }
        };

        carol.Orders = new List<Order> { carolOrder };

        var david = new Customer
        {
            CustomerId = 4,
            Name = "Hans Müller",
            Email = "hans.mueller@gmx.de",
            Country = "Germany"
        };

        var davidOrder = new Order
        {
            OrderId = 5,
            Customer = david,
            OrderDate = new DateTime(2026, 2, 28),
            Items = new List<OrderItem>
            {
                new() { ProductName = "Bluetooth Headphones", Quantity = 1, UnitPrice = 120m },
                new() { ProductName = "Phone Charger", Quantity = 2, UnitPrice = 25m }
            }
        };

        david.Orders = new List<Order> { davidOrder };

        return new List<Customer> { alice, bob, carol, david };
    }

    static void DisplayAllOrders(List<Customer> customers)
    {
        Console.WriteLine("All Orders:\n");

        foreach (var customer in customers)
        {
            Console.WriteLine($"Customer: {customer.Name} ({customer.Country})");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine();

            foreach (var order in customer.Orders)
            {
                Console.WriteLine($"  Order #{order.OrderId} - Date: {order.OrderDate:yyyy-MM-dd}");
                Console.WriteLine("  Items:");

                foreach (var item in order.Items)
                {
                    Console.WriteLine($"    - {item.ProductName}: {item.Quantity} × €{item.UnitPrice:F2} = €{item.GetTotalPrice():F2}");
                }

                var subtotal = order.GetSubtotal();
                var discount = order.GetDiscountAmount();
                var finalPrice = order.GetFinalPrice();

                Console.WriteLine($"  Subtotal: €{subtotal:F2}");

                if (discount > 0)
                {
                    Console.WriteLine($"  Discount (10%): -€{discount:F2}");
                    Console.WriteLine($"  Final Price: €{finalPrice:F2}");
                }
                else
                {
                    Console.WriteLine($"  Final Price: €{finalPrice:F2} (No discount - order ≤ €500)");
                }

                Console.WriteLine();
            }

            Console.WriteLine($"  Total Spent by {customer.Name}: €{customer.GetTotalSpent():F2}");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine();
        }
    }

    static void DisplayTopSpendingCustomer(List<Customer> customers)
    {
        Console.WriteLine("\nTop Spending Customer:\n");

        var topCustomerName = OrderService.FindTopSpendingCustomer(customers);
        var topCustomer = OrderService.FindTopSpendingCustomerObject(customers);

        if (topCustomer != null)
        {
            Console.WriteLine($"Name: {topCustomerName}");
            Console.WriteLine($"Country: {topCustomer.Country}");
            Console.WriteLine($"Total Orders: {topCustomer.Orders.Count}");
            Console.WriteLine($"Total Spent: €{topCustomer.GetTotalSpent():F2}");
            Console.WriteLine();
            Console.WriteLine("Congratulations on being our top customer!");
        }
        else
        {
            Console.WriteLine("No customers found.");
        }
    }
}

