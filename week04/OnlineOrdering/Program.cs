using System;
using System.Collections.Generic;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

   
    public override bool Equals(object obj)
    {
        if (obj is Product other)
            return Name == other.Name; 
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public void UpdateStock(int quantity)
    {
        Stock += quantity;
        Console.WriteLine($"Updated stock of {Name}. New stock: {Stock}");
    }

    public void GetInfo()
    {
        Console.WriteLine($"Product: {Name}, Price: ${Price}, Stock: {Stock}");
    }
}

public class Cart
{
    public Dictionary<Product, int> Items { get; set; } = new Dictionary<Product, int>();

    public void AddItem(Product product, int quantity)
    {
        if (Items.ContainsKey(product))
            Items[product] += quantity;
        else
            Items.Add(product, quantity);

        Console.WriteLine($"{quantity} x {product.Name} added to cart.");
    }

    public void RemoveItem(Product product)
    {
        if (Items.ContainsKey(product))
        {
            Items.Remove(product);
            Console.WriteLine($"{product.Name} removed from cart.");
        }
    }

    public decimal CalculateSubtotal()
    {
        decimal total = 0;
        foreach (var item in Items)
        {
            total += item.Key.Price * item.Value;
        }
        return total;
    }
}

public class Order
{
    public Customer Customer { get; set; }
    public Dictionary<Product, int> Items { get; set; } = new Dictionary<Product, int>();
    public decimal Total { get; set; }

    public void AddItem(Product product, int quantity)
    {
        if (Items.ContainsKey(product))
            Items[product] += quantity;
        else
            Items.Add(product, quantity);
    }

    public void CalculateTotal()
    {
        Total = 0;
        foreach (var item in Items)
        {
            Total += item.Key.Price * item.Value;
        }
        Console.WriteLine($"Order total: ${Total}");
    }

    public void ProcessPayment()
    {
        Console.WriteLine($"Processing payment of ${Total} for {Customer.Name}");
        foreach (var item in Items)
        {
            item.Key.Stock -= item.Value;
        }
        Console.WriteLine("Payment successful and stock updated.");
    }
}

public class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();

    public void PlaceOrder(Order order)
    {
        Orders.Add(order);
        Console.WriteLine($"{Name} placed an order.");
    }

    public void ViewOrders()
    {
        Console.WriteLine($"Orders for {Name}:");
        foreach (var order in Orders)
        {
            Console.WriteLine($"- Order with {order.Items.Count} items, Total: ${order.Total}");
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
       
        Product apple = new Product { Name = "Apple", Price = 0.5m, Stock = 100 };
        Product bread = new Product { Name = "Bread", Price = 2.0m, Stock = 50 };

        
        Cart cart = new Cart();
        cart.AddItem(apple, 5);
        cart.AddItem(bread, 2);
        Console.WriteLine($"Cart subtotal: ${cart.CalculateSubtotal()}");

       
        Customer customer = new Customer { Name = "Kishie", Email = "kishie@example.com" };

       
        Order order = new Order { Customer = customer };
        order.AddItem(apple, 5);
        order.AddItem(bread, 2);
        order.CalculateTotal();
        order.ProcessPayment();

       
        customer.PlaceOrder(order);
        customer.ViewOrders();
    }
}