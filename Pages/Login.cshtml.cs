using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SafeVaultWebApp.Tools;

namespace SafeVaultWebApp.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Username))
        {
            Message = "Username is required.";
            return Page();

        } else if (!InputValidator.IsValidInput(Username))
        {
            Message = "Invalid Username.";
            return Page();
        }

        if (string.IsNullOrEmpty(Password)) {
            Message = "Password is required.";
            return Page();
        } else if (!InputValidator.IsValidInput(Password))
        {
            Message = "Invalid Password.";
            return Page();
        }

        // Add your authentication logic here
        if (Username == "admin" && Password == "password") // Example logic
        {
            Message = "Login successful!";
            return RedirectToPage("/Success", new { username = Username });
        }
        else
        {
            Message = "Invalid username or password.";
            return Page();
        }
    }
}