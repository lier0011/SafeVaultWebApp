using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net; // Add this for password verification

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

    public IActionResult OnPost()
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

        Message = "Login successful!";
        return RedirectToPage("/Success", new { username = Username });
    }
}