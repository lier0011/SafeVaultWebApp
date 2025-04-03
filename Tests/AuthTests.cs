using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using SafeVaultWebApp.Services;
using SafeVaultWebApp.Models;
using SafeVaultWebApp.Data;

namespace SafeVaultWebApp.Tests;

[TestFixture]
public class AuthTests
{
    private AuthService _authService;
    private AppDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        // Configure in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new AppDbContext(options);

        // Seed test data
        _dbContext.Users.AddRange(
            new User { 
                UserID = 1, Username = "admin", 
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"), 
                Email = "admin@email", Role = "Admin" 
            },
            new User { 
                UserID = 2, Username = "user", 
                Password = BCrypt.Net.BCrypt.HashPassword("user123"), 
                Email = "user@email", Role = "User" 
            }
        );
        _dbContext.SaveChanges();

        // Initialize AuthService with the in-memory context
        _authService = new AuthService(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose of the in-memory database
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public void InvalidLoginAttempt_ShouldReturnNull()
    {
        // Arrange
        var invalidUsername = "invalidUser";
        var invalidPassword = "wrongPassword";

        // Act
        var result = _authService.Login(invalidUsername, invalidPassword);

        // Assert
        Assert.That(result, Is.Null, "Invalid login attempt should return null.");
    }

    [Test]
    public void ValidLoginAttempt_ShouldReturnTheUser()
    {
        // Arrange
        var validUsername = "admin";
        var validPassword = "admin123";

        // Act
        var result = _authService.Login(validUsername, validPassword);

        // Assert
        Assert.That(result, Is.InstanceOf<User>(), "Valid login attempt should return a User object.");
    }

    [Test]
    public void AdminAccess_ShouldAllowAccess()
    {
        // Arrange
        var adminUser = _dbContext.Users.First(u => u.Username == "admin");

        // Act
        var result = _authService.CheckAccess(adminUser, "Admin");

        // Assert
        Assert.That(result, Is.True, "Admin should have access.");
    }

    [Test]
    public void RegularUserAccess_ShouldDenyAccessToAdminResource()
    {
        // Arrange
        var regularUser = _dbContext.Users.First(u => u.Username == "user");

        // Act
        var result = _authService.CheckAccess(regularUser, "Admin");

        // Assert
        Assert.That(result, Is.False, "Regular user should not have access to admin resources.");
    }
}