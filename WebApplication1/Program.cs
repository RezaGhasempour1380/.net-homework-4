using Microsoft.EntityFrameworkCore;
using dbcontext; 
using mainclass;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using security; 
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add RazorPages with NewtonsoftJson configuration
builder.Services.AddRazorPages();

// Add controllers with NewtonsoftJson configuration
builder.Services.AddControllers();

builder.Logging.AddConsole();

// Add CORS (if needed for your client applications)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"] ?? ""))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["AuthToken"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Add DbContext configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Uncomment if you want to use HTTPS redirection in production
    // app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthentication(); // Ensure authentication middleware is used
app.UseAuthorization();
app.UseCors("AllowAll");

app.MapRazorPages();
app.MapControllers();

Console.WriteLine("Ready to serve requests...");

app.MapGet("/", () => Results.Redirect("/Login"));


// app.MapPost("/products", async (Product product, AppDbContext context) =>
// {
//     try
//     {
//         context.Product.Add(product);
//         await context.SaveChangesAsync();
//         return Results.Created($"/products/{product.id}", product);
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem("Failed to create a new product. " + ex.Message);
//     }
// });

// // app.MapGet("/products/{id}", async (int id, AppDbContext context) =>
// // {
// //     try
// //     {
// //         var product = await context.Product.FindAsync(id);
// //         if (product == null) return Results.NotFound("Product not found.");
// //         return Results.Ok(product);
// //     }
// //     catch (Exception ex)
// //     {
// //         return Results.Problem("Failed to fetch the product. " + ex.Message);
// //     }
// // });

// app.MapDelete("/products/{id}", async (int id, AppDbContext context) =>
// {
//     try
//     {
//         var product = await context.Product.FindAsync(id);
//         if (product == null)
//         {
//             return Results.NotFound($"Product with ID {id} not found.");
//         }

//         context.Product.Remove(product);
//         await context.SaveChangesAsync();
//         return Results.Ok($"Product with ID {id} deleted successfully.");
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem("Failed to delete the product. " + ex.Message);
//     }
// }).RequireAuthorization();

// app.MapPut("/products/{id}", async (int id, Product updatedProduct, AppDbContext context) =>
// {
//     var product = await context.Product.FindAsync(id);
//     if (product == null) return Results.NotFound($"Product with ID {id} not found.");

//     product.Name = updatedProduct.Name;
//     product.Price = updatedProduct.Price;

//     await context.SaveChangesAsync();
//     return Results.Ok(product);
// });

// app.MapGet("/products", async (AppDbContext context) =>
// {
//     try
//     {
//         var products = await context.Product.ToListAsync();
//         return Results.Ok(products);
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem("Failed to fetch products. " + ex.Message);
//     }
// });

// app.MapPost("/customers", async (Customer customer, AppDbContext context) =>
// {
//     try
//     {
//         context.Customer.Add(customer);
//         await context.SaveChangesAsync();
//         return Results.Created($"/customers/{customer.id}", customer);
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem("Failed to create a new customer. " + ex.Message);
//     }
// });

// app.MapDelete("/customers/{id}", async (int id, AppDbContext context) =>
// {
//     try
//     {
//         var customer = await context.Customer.FindAsync(id);
//         if (customer == null)
//         {
//             return Results.NotFound($"Customer with ID {id} not found.");
//         }

//         context.Customer.Remove(customer);
//         await context.SaveChangesAsync();
//         return Results.Ok($"Customer with ID {id} deleted successfully.");
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem("Failed to delete the customer. " + ex.Message);
//     }
// }).RequireAuthorization();

// app.MapPut("/customers/{id}", async (int id, Customer updatedCustomer, AppDbContext context) =>
// {
//     var customer = await context.Customer.FindAsync(id);
//     if (customer == null)
//     {
//         return Results.NotFound($"Customer with ID {id} not found.");
//     }

//     customer.Name = updatedCustomer.Name ?? customer.Name;
//     customer.Surname = updatedCustomer.Surname ?? customer.Surname;
//     customer.Email = updatedCustomer.Email ?? customer.Email;

//     await context.SaveChangesAsync();
//     return Results.Ok($"Customer with ID {id} updated successfully.");
// });

// // app.MapGet("/customers/{id}", async (int id, AppDbContext context) =>
// // {
// //     var customer = await context.Customer.FindAsync(id);
// //     if (customer == null)
// //     {
// //         return Results.NotFound($"Customer with ID {id} not found.");
// //     }
// //     return Results.Ok(customer);
// // });

// app.MapGet("/customers", async (AppDbContext context) =>
// {
//     try{
//         var customers = await context.Customer.ToListAsync();
//         return Results.Ok(customers);
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem("Failed to fetch customers. " + ex.Message);
//     }
// });

// app.MapPost("/orders", async (Orders order, AppDbContext context) =>
// {
//     // Optional: Validate the CustomerId exists
//     var customerExists = await context.Customer.AnyAsync(c => c.id == order.Customerid);
//     if (!customerExists)
//     {
//         return Results.NotFound($"Customer with ID {order.Customerid} not found.");
//     }

//     context.Orders.Add(order);
//     await context.SaveChangesAsync();
//     return Results.Created($"/orders/{order.id}", order);
// });

// app.MapDelete("/orders/{id}", async (int id, AppDbContext context) =>
// {
//     var order = await context.Orders.FindAsync(id);
//     if (order == null)
//     {
//         return Results.NotFound($"Order with ID {id} not found.");
//     }

//     context.Orders.Remove(order);
//     await context.SaveChangesAsync();
//     return Results.Ok($"Order with ID {id} deleted successfully.");
// }).RequireAuthorization();

// app.MapPut("/orders/{id}", async (int id, Orders updatedOrder, AppDbContext context) =>
// {
//     var order = await context.Orders.FindAsync(id);
//     if (order == null)
//     {
//         return Results.NotFound($"Order with ID {id} not found.");
//     }

//     order.Number = updatedOrder.Number ?? order.Number;
//     order.State = updatedOrder.State ?? order.State;
//     order.OrderDate = updatedOrder.OrderDate != default ? updatedOrder.OrderDate : order.OrderDate;
//     order.Customerid = updatedOrder.Customerid != 0 ? updatedOrder.Customerid : order.Customerid;

//     await context.SaveChangesAsync();
//     return Results.Ok($"Order with ID {id} updated successfully.");
// });

// // app.MapGet("/orders/{id}", async (int id, AppDbContext context) =>
// // {
// //     var order = await context.Orders//.Include(o => o.Customer)//.Include(o => o.OrderDetails)
// //                     .FirstOrDefaultAsync(o => o.id == id);
// //     if (order == null)
// //     {
// //         return Results.NotFound($"Order with ID {id} not found.");
// //     }
// //     return Results.Ok(order);
// // });

// app.MapGet("/orders", async (AppDbContext context) =>
// {
//     var orders = await context.Orders
//                               //.Include(o => o.Customer) // Include Customer details
//                               //.Include(o => o.OrderDetails) // Include OrderDetails
//                               .ToListAsync();
//     return Results.Ok(orders);
// });

// app.MapPost("/orderdetails", async (OrderDetails orderDetails, AppDbContext context) =>
// {
//     // Verify that the specified Order exists
//     var orderExists = await context.Orders.AnyAsync(o => o.id == orderDetails.Orderid);
//     if (!orderExists)
//     {
//         return Results.NotFound($"Order with ID {orderDetails.Orderid} not found.");
//     }

//     // Verify that the specified Product exists
//     var productExists = await context.Product.AnyAsync(p => p.id == orderDetails.Productid);
//     if (!productExists)
//     {
//         return Results.NotFound($"Product with ID {orderDetails.Productid} not found.");
//     }

//     context.OrderDetails.Add(orderDetails);
//     await context.SaveChangesAsync();
//     return Results.Created($"/orderdetails/{orderDetails.id}", orderDetails);
// });

// app.MapDelete("/orderdetails/{id}", async (int id, AppDbContext context) =>
// {
//     var orderDetails = await context.OrderDetails.FindAsync(id);
//     if (orderDetails == null)
//     {
//         return Results.NotFound($"OrderDetails with ID {id} not found.");
//     }

//     context.OrderDetails.Remove(orderDetails);
//     await context.SaveChangesAsync();
//     return Results.Ok($"OrderDetails with ID {id} deleted successfully.");
// }).RequireAuthorization();

// app.MapPut("/orderdetails/{id}", async (int id, OrderDetails updatedOrderDetails, AppDbContext context) =>
// {
//     var orderDetails = await context.OrderDetails.FindAsync(id);
//     if (orderDetails == null)
//     {
//         return Results.NotFound($"OrderDetails with ID {id} not found.");
//     }

//     orderDetails.Productid = updatedOrderDetails.Productid != 0 ? updatedOrderDetails.Productid : orderDetails.Productid;
//     orderDetails.Amount = updatedOrderDetails.Amount != 0 ? updatedOrderDetails.Amount : orderDetails.Amount;
//     orderDetails.Orderid = updatedOrderDetails.Orderid != 0 ? updatedOrderDetails.Orderid : orderDetails.Orderid;

//     await context.SaveChangesAsync();
//     return Results.Ok($"OrderDetails with ID {id} updated successfully.");
// });

// // app.MapGet("/orderdetails/{id}", async (int id, AppDbContext context) =>
// // {
// //     var orderDetail = await context.OrderDetails
// //                                    //.Include(od => od.Product)
// //                                    //.Include(od => od.Order)
// //                                    .FirstOrDefaultAsync(od => od.id == id);
// //     if (orderDetail == null)
// //     {
// //         return Results.NotFound($"OrderDetails with ID {id} not found.");
// //     }
// //     return Results.Ok(orderDetail);
// // });

// app.MapGet("/orderdetails", async (AppDbContext context) =>
// {
//     var orderDetailsList = await context.OrderDetails
//                                     //.Include(od => od.Order)
//                                     //.Include(od => od.Product)
//                                     .ToListAsync();
//     return Results.Ok(orderDetailsList);
// });

// app.MapGet("/orders-with-details", async (AppDbContext context) =>
// {
//     // Fetch all orders
//     var orders = await context.Orders.ToListAsync();
    
//     // Fetch all order details
//     var orderDetails = await context.OrderDetails.ToListAsync();

//     // Fetch all products
//     var products = await context.Product.ToDictionaryAsync(p => p.id, p => p);

//     var ordersWithDetails = orders.Select(order => new OrderDto
//     {
//         OrderId = order.id,
//         OrderNumber = order.Number,
//         OrderState = order.State,
//         OrderDate = order.OrderDate,
//         OrderItems = orderDetails
//             .Where(od => od.Orderid == order.id)
//             .Select(od => 
//             {
//                 var product = products.ContainsKey(od.Productid) ? products[od.Productid] : null;
//                 return new OrderItemDto
//                 {
//                     Amount = od.Amount,
//                     ProductName = product?.Name ?? "Unknown",
//                     ProductPrice = product?.Price ?? 0
//                 };
//             }).ToList()
//     }).ToList();

//     return Results.Ok(ordersWithDetails);
// });

// #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
// app.MapPost("/login", async (loginInfo loginDto, HttpContext httpContext) =>
// {
//     var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();
//     var validUsername = configuration["Credentials:Username"];
//     var validPassword = configuration["Credentials:Password"];

//     if (loginDto.Username == validUsername && loginDto.Password == validPassword)
//     {
// #pragma warning disable CS8604 // Possible null reference argument.
//         var token = JwtUtils.GenerateJwtToken(loginDto.Username, configuration);
// #pragma warning restore CS8604 // Possible null reference argument.
//         return Results.Ok(new { Token = token });
//     }
//     return Results.Unauthorized();
// });
// #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously


app.Run();
