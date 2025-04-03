using SafeVaultWebApp.Models;

namespace SafeVaultWebApp.Services;

public interface IAuthService
{
    // Method to simulate user login
    User? Login(string username, string password);

    // Method to check access control for a user
    bool CheckAccess(User user, string role);
}