using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using dbcontext; 
using mainclass;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
[ValidateAntiForgeryToken]

public class DashboardModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly ILogger<DashboardModel> _logger;

    private readonly TokenService _tokenService;

    public List<Product> Products { get; set; } = new List<Product>();

    public List<Customer> Customers { get; set; } = new List<Customer>();

    public List<OrderDetails> OrderDetail { get; set; } = new List<OrderDetails>();

    public List<Orders> Order { get; set; } = new List<Orders>();

    

    public DashboardModel(AppDbContext context, ILogger<DashboardModel> logger, TokenService tokenService)
    {
        _context = context;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task OnGet()
    {
        Products = await _context.Product.ToListAsync();
        Customers = await _context.Customer.ToListAsync();
        OrderDetail = await _context.OrderDetails.ToListAsync();
        Order = await _context.Orders.ToListAsync();
    }

    public async Task<IActionResult> OnPostAddProduct()
    {
        var newProduct = new Product
        {
            Name = Request.Form["newProductName"],
            Price = Convert.ToDecimal(Request.Form["newProductPrice"])
        };

        _context.Product.Add(newProduct);
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateProduct(int id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            product.Name = Request.Form["productName"];
            product.Price = Convert.ToDecimal(Request.Form["productPrice"]);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteProduct(int id)
    {
        var token = HttpContext.Request.Cookies["AuthToken"];
        if (!_tokenService.IsTokenValid(token))
        {
        return RedirectToPage("/Login");
        }
        else{

        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostAddCustomer()
    {
        var customer = new Customer
        {
            Name = Request.Form["newCustomerName"],
            Surname = Request.Form["newCustomerSurname"],
            Email = Request.Form["newCustomerEmail"]
        };

        _context.Customer.Add(customer);
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateCustomer(int id)
    {
        var customer = await _context.Customer.FindAsync(id);
        if (customer != null)
        {
            customer.Name = Request.Form["customerName"];
            customer.Surname = Request.Form["customerSurname"];
            customer.Email = Request.Form["customerEmail"];
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        return RedirectToPage(); // Optionally add an error message or handling
    }

    public async Task<IActionResult> OnPostDeleteCustomer(int id)
    {
        var token = HttpContext.Request.Cookies["AuthToken"];
        if (!_tokenService.IsTokenValid(token))
        {
        return RedirectToPage("/Login");
        }
        else{
        var customer = await _context.Customer.FindAsync(id);
        if (customer != null)
        {
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
        }
    }

        public async Task<IActionResult> OnPostAddOrder()
    {
        var order = new Orders
        {
            Number = Request.Form["orderNumber"],
            State = Request.Form["orderState"],
            OrderDate = DateTime.Parse(Request.Form["orderDate"]),
            Customerid = int.Parse(Request.Form["customerId"])
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            order.Number = Request.Form["orderNumber"];
            order.State = Request.Form["orderState"];
            order.OrderDate = DateTime.Parse(Request.Form["orderDate"]);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        return RedirectToPage(); // Optionally add an error message or handling
    }

    public async Task<IActionResult> OnPostDeleteOrder(int id)
    {
        var token = HttpContext.Request.Cookies["AuthToken"];
        if (!_tokenService.IsTokenValid(token))
        {
        return RedirectToPage("/Login");
        }
        else{
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostAddOrderDetail()
    {
        var orderDetail = new OrderDetails
        {
            Productid = int.Parse(Request.Form["productId"]),
            Amount = int.Parse(Request.Form["amount"]),
            Orderid = int.Parse(Request.Form["orderId"])
        };

        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateOrderDetail(int id)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail != null)
        {
            orderDetail.Productid = int.Parse(Request.Form["productId"]);
            orderDetail.Amount = int.Parse(Request.Form["amount"]);
            orderDetail.Orderid = int.Parse(Request.Form["orderId"]);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        return RedirectToPage(); // Optionally add an error message or handling
    }

    public async Task<IActionResult> OnPostDeleteOrderDetail(int id)
    {
        var token = HttpContext.Request.Cookies["AuthToken"];
        if (!_tokenService.IsTokenValid(token))
        {
        return RedirectToPage("/Login");
        }
        else{
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail != null)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
        }
    }
}
