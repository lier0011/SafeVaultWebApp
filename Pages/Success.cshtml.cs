using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SafeVaultWebApp.Pages;

public class SuccessModel : PageModel
{
    public string? Message { get; private set; }

    public void OnGet(string message)
    {
        Message = message;
    }
}
