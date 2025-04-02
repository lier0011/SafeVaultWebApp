using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            Message = "Username and Password are required.";
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