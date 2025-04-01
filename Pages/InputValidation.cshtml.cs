using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SafeVaultWebApp.Data;
using SafeVaultWebApp.Models;
using System.Text.RegularExpressions;

namespace SafeVaultWebApp.Pages;

public class InputValidationModel : PageModel
{
    private readonly ILogger<InputValidationModel> _logger;
    private readonly AppDbContext _dbContext;

    public InputValidationModel(ILogger<InputValidationModel> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [BindProperty]
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(10, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 10 characters.")]
    public string Username { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 20 characters.")]
    public string Email { get; set; }

    public string Message { get; private set; }

    public void OnGet()
    {
        // ...existing code...
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Check for potentially harmful input
        if (ContainsHarmfulInput(Username) || ContainsHarmfulInput(Email))
        {
            ModelState.AddModelError(string.Empty, "Input contains potentially harmful content.");
            return Page();
        }

        // Save the sanitized data to the database using AppDbContext
        var user = new User
        {
            Username = Username,
            Email = Email
        };
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        Message = "Form submitted successfully!";
        return RedirectToPage("/Success", new { username = Username });
    }

    private bool ContainsHarmfulInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        // Check for HTML tags
        if (Regex.IsMatch(input, @"<.*?>"))
        {
            return true;
        }

        // Check for SQL keywords or patterns
        string[] sqlKeywords = { "SELECT", "INSERT", "DELETE", "UPDATE", "DROP", "--", ";" };
        foreach (var keyword in sqlKeywords)
        {
            if (input.ToUpper().Contains(keyword))
            {
                return true;
            }
        }

        return false;
    }
}