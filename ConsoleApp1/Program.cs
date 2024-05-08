using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using mainclass;


class Program
{
    
    static HttpClient client = new HttpClient();
    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to the application!");
        Console.WriteLine("do you want to login? y/n");
        string ?login = Console.ReadLine();
        if (login == "y")
        {
            await Login();
        }
        else
        {
            Console.WriteLine("You can't use delete from database without logging in.");
            
        }

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1: Add a new product");
            Console.WriteLine("2: Delete a product");
            Console.WriteLine("3: Update a product");
            Console.WriteLine("4: Fetch a product by ID");
            Console.WriteLine("5: Fetch all products");
            Console.WriteLine("6: Add a new customer");
            Console.WriteLine("7: Delete a customer");
            Console.WriteLine("8: Update a customer");
            Console.WriteLine("9: Fetch a customer by ID");
            Console.WriteLine("10: Fetch all customers");
            Console.WriteLine("11: Add a new order");
            Console.WriteLine("12: Delete an order");
            Console.WriteLine("13: Update an order");
            Console.WriteLine("14: Fetch an order by ID");
            Console.WriteLine("15: Fetch all orders");
            Console.WriteLine("16: Add OrderDetails");
            Console.WriteLine("17: Delete OrderDetails");
            Console.WriteLine("18: Update OrderDetails");
            Console.WriteLine("19: Fetch an OrderDetails by ID");
            Console.WriteLine("20: Fetch all OrderDetails");
            Console.WriteLine("21: Fetch all orders with details");
            Console.WriteLine("0: Exit");

            string ?option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await AddProduct();
                    break;
                case "2":
                    await DeleteProduct();
                    break;
                case "3":
                    await UpdateProduct();
                    break;
                case "4":
                    await FetchProduct();
                    break;
                case "5":
                    await FetchAllProducts();
                    break;
                case "6":
                    await AddCustomer();
                    break;
                case "7":
                    await DeleteCustomer();
                    break;
                case "8":
                    await UpdateCustomer();
                    break;
                case "9":
                    await FetchCustomer();
                    break;
                case "10":
                    await FetchAllCustomers();
                    break;
                case "11":
                    await AddOrder();
                    break;
                case "12":
                    await DeleteOrder();
                    break;
                case "13":
                    await UpdateOrder();
                    break;
                case "14":
                    await FetchOrder();
                    break;
                case "15":
                    await FetchAllOrders();
                    break;
                case "16":
                    await AddOrderDetails();
                    break;
                case "17":
                    await DeleteOrderDetails();
                    break;
                case "18":
                    await UpdateOrderDetails();
                    break;
                case "19":
                    await FetchOrderDetails();
                    break;
                case "20":
                    await FetchAllOrderDetails();
                    break;
                case "21":
                    await FetchOrdersWithDetails();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
                if (!exit)
            {
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine(); // Wait for user to press Enter
            }
        }
    }

    static async Task AddProduct()
    {
        Console.WriteLine("Enter product name:");
        string ?name = Console.ReadLine();

        Console.WriteLine("Enter product price:");
        decimal price = decimal.Parse(Console.ReadLine() ?? "0");

        var product = new { Name = name, Price = price };
        
        string productJson = JsonSerializer.Serialize(product);
        var content = new StringContent(productJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("http://localhost:5117/products", content);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Product added successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to add product. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task DeleteProduct()
    {
        Console.WriteLine("Enter product ID to delete:");
        int id = int.Parse(Console.ReadLine() ?? "0");

        try
        {
        using var response = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5117/products/{id}");
        // Add the Authorization header with the Bearer token
        response.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalSettings.AuthToken);
        using HttpResponseMessage response2 = await client.SendAsync(response);
            
            if (response2.IsSuccessStatusCode)
            {
                Console.WriteLine("Product deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to delete product. Status code: {response2.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task UpdateProduct()
    {
        Console.WriteLine("Enter product ID to update:");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Enter new product name:");
        string ?name = Console.ReadLine();

        Console.WriteLine("Enter new product price:");
        decimal price = decimal.Parse(Console.ReadLine() ?? "0");

        var product = new { Name = name, Price = price };

        string productJson = JsonSerializer.Serialize(product);
        var content = new StringContent(productJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PutAsync($"http://localhost:5117/products/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Product updated successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to update product. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchProduct()
    {
        Console.WriteLine("Enter product ID to fetch:");
        int id = int.Parse(Console.ReadLine() ?? "0");

        try
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5117/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Product details: {responseBody}");
            }
            else
            {
                Console.WriteLine($"Failed to fetch product. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchAllProducts()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5117/products");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Products:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch products. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task AddCustomer()
    {
        Console.WriteLine("Enter customer name:");
        string ?name = Console.ReadLine();

        Console.WriteLine("Enter customer surname:");
        string ?surname = Console.ReadLine();

        Console.WriteLine("Enter customer email:");
        string ?email = Console.ReadLine();

        var customer = new { Name = name, Surname = surname, Email = email };
        
        string customerJson = JsonSerializer.Serialize(customer);
        var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("http://localhost:5117/customers", content);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Customer added successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to add customer. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task DeleteCustomer()
    {
        Console.WriteLine("Enter customer ID to delete:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer.");
        }

        try
        {
        using var response = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5117/customers/{id}");
        // Add the Authorization header with the Bearer token
        response.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalSettings.AuthToken);
        using HttpResponseMessage response2 = await client.SendAsync(response);
            
            if (response2.IsSuccessStatusCode)
            {
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to delete customer. Status code: {response2.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task UpdateCustomer()
    {
        Console.WriteLine("Enter customer ID to update:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer.");
        }

        Console.WriteLine("Enter customer's new name (press enter to skip):");
        string ?name = Console.ReadLine();

        Console.WriteLine("Enter customer's new surname (press enter to skip):");
        string ?surname = Console.ReadLine();

        Console.WriteLine("Enter customer's new email (press enter to skip):");
        string ?email = Console.ReadLine();

        var customer = new 
        { 
            Name = string.IsNullOrEmpty(name) ? null : name, 
            Surname = string.IsNullOrEmpty(surname) ? null : surname, 
            Email = string.IsNullOrEmpty(email) ? null : email 
        };
        
        string customerJson = JsonSerializer.Serialize(customer);
        var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PutAsync($"http://localhost:5117/customers/{id}", content);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Customer updated successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to update customer. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchCustomer()
    {
        Console.WriteLine("Enter customer ID to fetch:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer.");
        }

        try
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5117/customers/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Customer details:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch customer. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchAllCustomers()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5117/customers");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Customers:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch customers. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task AddOrder()
    {
        Console.WriteLine("Enter the order number:");
        string ?number = Console.ReadLine();

        Console.WriteLine("Enter the order state:");
        string ?state = Console.ReadLine();

        Console.WriteLine("Enter the order date (yyyy-mm-dd):");
        DateTime orderDate;
        while (!DateTime.TryParse(Console.ReadLine(), out orderDate))
        {
            Console.WriteLine("Invalid date, please enter in yyyy-mm-dd format:");
        }

        Console.WriteLine("Enter Customer ID for the order:");
        int customerId;
        while (!int.TryParse(Console.ReadLine(), out customerId))
        {
            Console.WriteLine("Please enter a valid integer for Customer ID.");
        }

        var order = new { Number = number, State = state, OrderDate = orderDate, Customerid = customerId };

        string orderJson = JsonSerializer.Serialize(order);
        var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("http://localhost:5117/orders", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Order added successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to add order. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task DeleteOrder()
    {
        Console.WriteLine("Enter order ID to delete:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer for Order ID.");
        }

        try
        {
        using var response = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5117/orders/{id}");
        // Add the Authorization header with the Bearer token
        response.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalSettings.AuthToken);
        using HttpResponseMessage response2 = await client.SendAsync(response);
            
            if (response2.IsSuccessStatusCode)
            {
                Console.WriteLine("Order deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to delete order. Status code: {response2.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task UpdateOrder()
    {
        Console.WriteLine("Enter order ID to update:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer for Order ID.");
        }

        Console.WriteLine("Enter new order number (press enter to skip):");
        string ?number = Console.ReadLine();

        Console.WriteLine("Enter new order state (press enter to skip):");
        string ?state = Console.ReadLine();

        Console.WriteLine("Enter new order date in format yyyy-mm-dd (press enter to skip):");
        DateTime orderDate;
        DateTime.TryParse(Console.ReadLine(), out orderDate); // Default will be DateTime.MinValue if parsing fails

        Console.WriteLine("Enter new Customer ID for the order (enter 0 to skip):");
        int customerId;
        int.TryParse(Console.ReadLine(), out customerId); // Default will be 0 if parsing fails

        var order = new 
        { 
            Number = string.IsNullOrWhiteSpace(number) ? null : number, 
            State = string.IsNullOrWhiteSpace(state) ? null : state, 
            OrderDate = orderDate == DateTime.MinValue ? (DateTime?)null : orderDate, // Send null if parsing failed
            Customerid = customerId == 0 ? (int?)null : customerId // Send null if input is 0
        };

        string orderJson = JsonSerializer.Serialize(order);
        var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PutAsync($"http://localhost:5117/orders/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Order updated successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to update order. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchOrder()
    {
        Console.WriteLine("Enter order ID to fetch:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer for Order ID.");
        }

        try
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5117/orders/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Order details:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch order. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchAllOrders()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5117/orders");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("All Orders:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch orders. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task AddOrderDetails()
    {
        Console.WriteLine("Enter Order ID:");
        int orderId;
        while (!int.TryParse(Console.ReadLine(), out orderId))
        {
            Console.WriteLine("Please enter a valid integer for Order ID.");
        }

        Console.WriteLine("Enter Product ID:");
        int productId;
        while (!int.TryParse(Console.ReadLine(), out productId))
        {
            Console.WriteLine("Please enter a valid integer for Product ID.");
        }

        Console.WriteLine("Enter Amount:");
        int amount;
        while (!int.TryParse(Console.ReadLine(), out amount))
        {
            Console.WriteLine("Please enter a valid integer for Amount.");
        }

        var orderDetails = new { Orderid = orderId, Productid = productId, Amount = amount };

        string orderDetailsJson = JsonSerializer.Serialize(orderDetails);
        var content = new StringContent(orderDetailsJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("http://localhost:5117/orderdetails", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("OrderDetails added successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to add OrderDetails. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task DeleteOrderDetails()
    {
        Console.WriteLine("Enter OrderDetails ID to delete:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer for OrderDetails ID.");
        }

        try
        {
        using var response = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5117/orderdetails/{id}");
        // Add the Authorization header with the Bearer token
        response.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalSettings.AuthToken);
        using HttpResponseMessage response2 = await client.SendAsync(response);
            
            if (response2.IsSuccessStatusCode)
            {
                Console.WriteLine("OrderDetails deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to delete OrderDetails. Status code: {response2.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task UpdateOrderDetails()
    {
        Console.WriteLine("Enter OrderDetails ID to update:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer for OrderDetails ID.");
        }

        Console.WriteLine("Enter new Product ID (0 to skip):");
        int productId;
        int.TryParse(Console.ReadLine(), out productId);

        Console.WriteLine("Enter new Amount (0 to skip):");
        int amount;
        int.TryParse(Console.ReadLine(), out amount);

        Console.WriteLine("Enter new Order ID (0 to skip):");
        int orderId;
        int.TryParse(Console.ReadLine(), out orderId);

        var orderDetails = new { Productid = productId, Amount = amount, Orderid = orderId };

        string orderDetailsJson = JsonSerializer.Serialize(orderDetails);
        var content = new StringContent(orderDetailsJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PutAsync($"http://localhost:5117/orderdetails/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("OrderDetails updated successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to update OrderDetails. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchOrderDetails()
    {
        Console.WriteLine("Enter OrderDetails ID to fetch:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer for OrderDetails ID.");
        }

        try
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5117/orderdetails/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("OrderDetails:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch OrderDetails. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchAllOrderDetails()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5117/orderdetails");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("All OrderDetails:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch OrderDetails. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static async Task FetchOrdersWithDetails()
{
    try
    {
        HttpResponseMessage response = await client.GetAsync("http://localhost:5117/orders-with-details");
        
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var ordersWithDetails = JsonSerializer.Deserialize<List<OrderDto>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var order in ordersWithDetails)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Number: {order.OrderNumber}, State: {order.OrderState}, Date: {order.OrderDate:yyyy-MM-dd}");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"\tProduct Name: {item.ProductName}, Price: {item.ProductPrice}, Amount: {item.Amount}");
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    Console.WriteLine();
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        else
        {
            Console.WriteLine("Failed to fetch orders with details. " +
                              $"Status code: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

    static async Task Login()
{
    Console.WriteLine("Enter username:");
    string username = Console.ReadLine() ?? string.Empty;

    Console.WriteLine("Enter password:");
    string password = Console.ReadLine() ?? string.Empty;

    var login = new loginInfo { Username = username, Password = password };

    string json = JsonSerializer.Serialize(login);
    var data = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync("http://localhost:5117/login", data);

    if (response.IsSuccessStatusCode)
    {
        var token = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var tokenData = JsonSerializer.Deserialize<TokenData>(token, options);
        if (tokenData != null && tokenData.Token != null)
        {
            GlobalSettings.AuthToken = tokenData.Token;
            //Console.WriteLine("Token stored: " + GlobalSettings.AuthToken);
        }
        else
        {
            Console.WriteLine("Token data is null or missing.");
        }
        Console.WriteLine("Login successful.");
    }
    else
    {
        Console.WriteLine("Login failed.");
    }
}
}
