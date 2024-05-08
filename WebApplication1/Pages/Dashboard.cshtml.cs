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

    public DashboardModel(AppDbContext context, ILogger<DashboardModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task OnGet()
    {
        Products = await _context.Product.ToListAsync();
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
}
