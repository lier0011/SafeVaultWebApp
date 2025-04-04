using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using SafeVaultWebApp.Data;
using SafeVaultWebApp.Tools;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SafeVaultWebApp.Pages;

[ValidateAntiForgeryToken]
public class InputValidationModel : PageModel
{
    private readonly ILogger<InputValidationModel> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public InputValidationModel(ILogger<InputValidationModel> logger, AppDbContext dbContext, IConfiguration configuration)
    {
        _logger = logger;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [BindProperty]
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(10, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 10 characters.")]
    public required string Username { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 20 characters.")]
    public required string Email { get; set; }

    public string? Message { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public bool IsAuthorized { get; private set; }

    public void OnGet()
    {
        IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
        /*if (IsAuthenticated)
        {
            Username = User.FindFirst(ClaimTypes.Name)?.Value ?? "User";
            Console.WriteLine(User.IsInRole("admin"));
        }*/
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Check for potentially harmful input
        if (InputValidator.IsValidInput(Username) == false)
        {
            ModelState.AddModelError(string.Empty, "Invalid input: username");
            return Page();
        }
        if (InputValidator.IsValidInput(Email) == false)
        {
            ModelState.AddModelError(string.Empty, "Invalid input: email");
            return Page();
        }

        // Securely retrieve user information to ensure no duplicates
        var existingUser = _dbContext.Users
            .Where(u => u.Username == Username && u.Email == Email)
            .FirstOrDefault();

        // example of secure parameterised query for the above "existingUser" handling
        string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Email = @Email";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        int countUser = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@Email", Email);
                countUser = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        Console.WriteLine($"countUser: {countUser}");
        if (existingUser != null && countUser > 0)
        {
            ModelState.AddModelError(string.Empty, "Valid user");
            return Page();
        } else {
            ModelState.AddModelError(string.Empty, "Invalid user");
            return Page();
        }
    }
}