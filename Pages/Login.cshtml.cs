using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net; // Add this for password verification
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using SafeVaultWebApp.Data;
using SafeVaultWebApp.Tools;

namespace SafeVaultWebApp.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public LoginModel(ILogger<LoginModel> logger, AppDbContext dbContext, IConfiguration configuration)
    {
        _logger = logger;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [BindProperty]
    public required string Username { get; set; }

    [BindProperty]
    public required string Password { get; set; }

    public string? Message { get; set; }
    public bool IsAuthenticated { get; private set; }

    public void OnGet()
    {
        IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
        if (IsAuthenticated)
        {
            Username = User.FindFirst(ClaimTypes.Name)?.Value ?? "User";
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Username))
        {
            Message = "Username is required.";
            return Page();
        }
        else if (!InputValidator.IsValidInput(Username))
        {
            Message = "Invalid Username.";
            return Page();
        }

        if (string.IsNullOrEmpty(Password))
        {
            Message = "Password is required.";
            return Page();
        }
        else if (!InputValidator.IsValidInput(Password))
        {
            Message = "Invalid Password.";
            return Page();
        }

        // Authenticate user
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
        {
            Message = "Invalid username or password.";
            return Page();
        }

        // Set the username in the authentication cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Username)
        };

        // Check if the user has the "Admin" role and add the claim
        if (user.Role.ToUpper() == "ADMIN")
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToUpper()));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        Message = "Login successful!";
        return RedirectToPage("/Success", new { username = Username });
    }
}