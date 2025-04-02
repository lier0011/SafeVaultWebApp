using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SafeVaultWebApp.Tools;
using SafeVaultWebApp.Data; // Assuming this namespace contains the database context
using SafeVaultWebApp.Models; // Assuming this namespace contains the User model
using BCrypt.Net; // Add this for password hashing

namespace SafeVaultWebApp.Pages;

public class RegisterModel : PageModel
{
    private readonly ILogger<RegisterModel> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    public RegisterModel(ILogger<RegisterModel> logger, AppDbContext dbContext, IConfiguration configuration)
    {
        _logger = logger;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [BindProperty]
    public required string Username { get; set; }

    [BindProperty]
    public required string Email { get; set; }

    [BindProperty]
    public required string Role { get; set; }

    [BindProperty]
    public required string Password { get; set; }

    public string? Message { get; set; }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Username))
        {
            ModelState.AddModelError(nameof(Username), "Username is required.");
        }
        else if (!InputValidator.IsValidInput(Username))
        {
            ModelState.AddModelError(nameof(Username), "Invalid Username.");
        }

        if (string.IsNullOrEmpty(Email))
        {
            ModelState.AddModelError(nameof(Email), "Email is required.");
        }
        else if (!InputValidator.IsValidInput(Email))
        {
            ModelState.AddModelError(nameof(Email), "Invalid Email.");
        }

        if (string.IsNullOrEmpty(Role))
        {
            ModelState.AddModelError(nameof(Role), "Role is required.");
        }
        else if (!InputValidator.IsValidInput(Role))
        {
            ModelState.AddModelError(nameof(Role), "Invalid Role.");
        }

        if (string.IsNullOrEmpty(Password))
        {
            ModelState.AddModelError(nameof(Password), "Password is required.");
        }
        else if (!InputValidator.IsValidInput(Password))
        {
            ModelState.AddModelError(nameof(Password), "Invalid Password.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Hash the password before saving
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

        // Save user to the database
        var user = new User
        {
            Username = Username,
            Email = Email,
            Role = Role,
            Password = hashedPassword
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        Message = "Registration successful!";
        return RedirectToPage("/Success", new { username = Username });
    }
}
