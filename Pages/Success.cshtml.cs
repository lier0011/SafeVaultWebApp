using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SafeVaultWebApp.Pages;

public class SuccessModel : PageModel
{
    public string Username { get; private set; }

    public void OnGet(string username)
    {
        Username = username;
    }
}
