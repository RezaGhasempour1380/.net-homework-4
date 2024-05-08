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

    public List<Product> Products { get; set; } = new List<Product>();

    public List<Customer> Customers { get; set; } = new List<Customer>();

    public List<OrderDetails> OrderDetail { get; set; } = new List<OrderDetails>();

    public List<Orders> Order { get; set; } = new List<Orders>();

    

    public DashboardModel(AppDbContext context, ILogger<DashboardModel> logger)
    {
        _context = context;
        _logger = logger;
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
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
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
        var customer = await _context.Customer.FindAsync(id);
        if (customer != null)
        {
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
