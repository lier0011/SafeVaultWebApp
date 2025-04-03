namespace SafeVaultWebApp.Tests.Services;

public interface IAuthService
{

    // Method to simulate user login
    bool Login(string username, string password);

    // Method to check access control for a user
    bool CheckAccess(int userId);
}