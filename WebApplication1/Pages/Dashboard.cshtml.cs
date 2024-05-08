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
    public string StatusMessage { get; set; }

    [BindProperty]
    public Product NewProduct { get; set; } = new Product();
    [BindProperty]
    public Product UpdatedProduct { get; set; } = new Product();

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

    public async Task<JsonResult> OnPostAddProduct( NewProductModel newProductModel)
    {
        if (!ModelState.IsValid)
        {
            return new JsonResult(new { success = false, message = "Invalid product data" });
        }

        try
        {
            var product = new Product 
            {
                Name = newProductModel.Name,
                Price = newProductModel.Price
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Product added successfully." });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

        public async Task<JsonResult> OnPostUpdateProduct(UpdateProductModel model)
        {
            if (model == null) {
                return new JsonResult(new { success = false, message = "Invalid product data" });
            }

            var product = await _context.Product.FindAsync(model.Id);
            if (product != null)
            {
                product.Name = model.UpdatedProductName;
                product.Price = model.UpdatedProductPrice;
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Product updated successfully." });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Product not found." });
            }
        }
    
    public async Task<JsonResult> OnPostDeleteProduct(int id)
    {
        _logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);
        var product = await _context.Product.FindAsync(id);

        if (product != null)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product deleted successfully: {ProductId}", id);
            return new JsonResult(new { success = true, message = "Product deleted successfully" });
        }
        else
        {
            _logger.LogWarning("Product not found for ID: {ProductId}", id);
            return new JsonResult(new { success = false, message = "Product not found" });
        }
    }
}
