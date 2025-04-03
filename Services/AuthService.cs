using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

using SafeVaultWebApp.Data;
using SafeVaultWebApp.Models;

namespace SafeVaultWebApp.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;

    public AuthService(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    // Method to simulate user login
    public User? Login(string username, string password) {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }
        return user;
    }

    // Method to check access control for a user
    public bool CheckAccess(User user, string role) {
        if (user.Role.ToUpper() != role.ToUpper()) {
            return false;
        }

        return true;
    }
}