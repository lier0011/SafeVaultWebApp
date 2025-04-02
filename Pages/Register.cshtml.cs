using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SafeVaultWebApp.Tools;

namespace SafeVaultWebApp.Pages;

public class RegisterModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Role { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

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

        // Add your registration logic here (e.g., save user to the database)
        Message = "Registration successful!";
        return RedirectToPage("/Success", new { username = Username });
    }
}
