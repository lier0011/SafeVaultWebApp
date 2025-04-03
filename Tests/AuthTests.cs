using NUnit.Framework;
using Moq;

using SafeVaultWebApp.Tests.Services; // Assuming you have a service for authentication
using SafeVaultWebApp.Models;   // Assuming you have models for user data

namespace SafeVaultWebApp.Tests;

[TestFixture]
public class AuthTests
{
    private Mock<IAuthService> _authServiceMock;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
    }

    [Test]
    public void InvalidLoginAttempt_ShouldReturnFalse()
    {
        // Arrange
        var invalidUsername = "invalidUser";
        var invalidPassword = "wrongPassword";
        _authServiceMock.Setup(s => s.Login(invalidUsername, invalidPassword)).Returns(false);

        // Act
        var result = _authServiceMock.Object.Login(invalidUsername, invalidPassword);

        // Assert
        Assert.That(result, Is.False, "Invalid login attempt should return false.");
    }

    [Test]
    public void UnauthorizedAccess_ShouldThrowException()
    {
        // Arrange
        var unauthorizedUserId = 999; // Simulate a user ID with no access
        _authServiceMock.Setup(s => s.CheckAccess(unauthorizedUserId))
            .Throws(new UnauthorizedAccessException());

        // Act & Assert
        Assert.That(() => _authServiceMock.Object.CheckAccess(unauthorizedUserId), 
            Throws.TypeOf<UnauthorizedAccessException>(), 
            "Unauthorized access should throw an exception.");
    }

    [Test]
    public void AdminAccess_ShouldAllowAccess()
    {
        // Arrange
        var adminUserId = 1; // Simulate an admin user ID
        _authServiceMock.Setup(s => s.CheckAccess(adminUserId)).Returns(true);

        // Act
        var result = _authServiceMock.Object.CheckAccess(adminUserId);

        // Assert
        Assert.That(result, Is.True, "Admin should have access.");
    }

    [Test]
    public void RegularUserAccess_ShouldDenyAccessToRestrictedResource()
    {
        // Arrange
        var regularUserId = 2; // Simulate a regular user ID
        _authServiceMock.Setup(s => s.CheckAccess(regularUserId)).Returns(false);

        // Act
        var result = _authServiceMock.Object.CheckAccess(regularUserId);

        // Assert
        Assert.That(result, Is.False, "Regular user should not have access to restricted resources.");
    }
}