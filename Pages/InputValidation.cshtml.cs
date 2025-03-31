using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace SafeVaultWebApp.Pages;

public class InputValidationModel : PageModel
{
    private readonly ILogger<InputValidationModel> _logger;

    public InputValidationModel(ILogger<InputValidationModel> logger)
    {
        _logger = logger;
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

        // Process the form data (e.g., save to database)
        Message = "Form submitted successfully!";
        return RedirectToPage("/Success", new { username = Username });
    }
}