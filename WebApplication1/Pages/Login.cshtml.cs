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
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty(SupportsGet = false)]
    public string? Username { get; set; }
    [BindProperty(SupportsGet = false)]
    public string? Password { get; set; }

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPost()
    {
            if (!ModelState.IsValid)
        {
            Console.WriteLine("Invalid model state");
            return Page();  // or return error message to view
        }
        _logger.LogInformation($"Received username: {Username}, password: {Password}");
        var validUsername = _configuration["Credentials:Username"];
        var validPassword = _configuration["Credentials:Password"];
        Console.WriteLine("Username: " + Username);
        Console.WriteLine("Password: " + Password);

        if (Username == validUsername && Password == validPassword)
        {
            var token = JwtUtils.GenerateJwtToken(Username, _configuration);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true, 
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("AuthToken", token, cookieOptions);
            return RedirectToPage("Dashboard"); // Redirect to a secure page
        }
        else
        {
            ErrorMessage = "Invalid username or password";
            return Page();
        }
    }
}
